using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.EnumDTOs;
using Shipping.BusinessLogicLayer.DTOs.OrderDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Services 
{
    public class OrderService : IOrderService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGeneralSettingsService _generalSetting;
        public OrderService(UnitOfWork unitOfWork, IMapper mapper, IGeneralSettingsService generalSetting)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _generalSetting = generalSetting;
        }
        public async Task<PagedResponse<ReadOrderDTO>> GetAllOrdersAsync(PaginationDTO pagination)
        {
            var orders = _unitOfWork.OrderRepo.GetAll().Where(o=> o.IsDeleted == false);

            // فلترة بالـ status لو موجود
            if (pagination.Status.HasValue)
                orders = orders.Where(o => (int)o.Status == pagination.Status.Value);

            //var orders = _unitOfWork.OrderRepo.GetAll();
            var count = orders.Count();

            var pagedOrders = orders
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            var readOrders = _mapper.Map<List<ReadOrderDTO>>(pagedOrders);
            var result = new PagedResponse<ReadOrderDTO>
            {
                Items = readOrders,
                TotalCount = count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize)
            };

            return result;
        }

        public async Task<List<ReadOrderDTO>> GetAllWithoutPagination()
        {
            var orders = _unitOfWork.OrderRepo.GetAll().Where(o => o.IsDeleted == false).ToList();
            return _mapper.Map<List<ReadOrderDTO>>(orders);
        }

        public async Task<ReadOneOrderDTO> GetOrderById(int id)
        {
            var order = _unitOfWork.OrderRepo.GetById(id);
            var product = order.Products;
            var mapped = _mapper.Map<ReadOneOrderDTO>(order);
            mapped.deliveryManId = order.DeliveryAgentId;
            mapped.StatusId = (int)order.Status;
            mapped.ShippingTypeID = (int)order.ShippingType;
            mapped.PaymentTypeId = (int)order.PaymentType;
            return mapped;
        }

        public async Task AddOrder(AddOrderDTO orderDTO)
        {
            decimal shippingCost = CalculateOrderShippingCost(orderDTO).Result;
            var totalCost = shippingCost + orderDTO.Products.Sum(p => p.Price * p.Quantity);
            var totalWeight = orderDTO.Products.Sum(p => p.Weight * p.Quantity);

            var order = _mapper.Map<Order>(orderDTO);
            order.ShippingCost = shippingCost;
            order.TotalCost = totalCost;
            order.TotalWeight = totalWeight;
            order.Products = _mapper.Map<List<Product>>(orderDTO.Products);

            _unitOfWork.OrderRepo.Add(order);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateOrder(UpdateOrderDTO updateOrderDTO)
        {
            var order = _unitOfWork.OrderRepo.GetById(updateOrderDTO.Id);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.Notes = updateOrderDTO.Notes ?? order.Notes;
            order.CustomerName = updateOrderDTO.CustomerName ?? order.CustomerName;
            order.CustomerPhone = updateOrderDTO.CustomerPhone ?? order.CustomerPhone;
            order.IsShippedToVillage = updateOrderDTO.IsShippedToVillage ?? order.IsShippedToVillage;
            order.Address = updateOrderDTO.Address ?? order.Address;
            order.Status = updateOrderDTO.Status ?? order.Status;
            order.ShippingType = updateOrderDTO.ShippingType ?? order.ShippingType;
            order.OrderType = updateOrderDTO.OrderType ?? order.OrderType;
            order.PaymentType = updateOrderDTO.PaymentType ?? order.PaymentType;

            order.IsPickup = updateOrderDTO.IsPickup ?? order.IsPickup;
            order.IsActive = updateOrderDTO.IsActive ?? order.IsActive;
            if (updateOrderDTO.DeliveryManId != 0)
            {
                var deliveryAgentExists =  _unitOfWork.DeliveryManRepo.GetAll()
                    .Any(d => d.Id == updateOrderDTO.DeliveryManId.Value && d.User.IsDeleted == false);

                if (!deliveryAgentExists)
                {
                    throw new Exception($"Delivery agent with ID {updateOrderDTO.DeliveryManId.Value} does not exist.");
                }

                order.DeliveryAgentId = updateOrderDTO.DeliveryManId.Value;
            }
            if (updateOrderDTO.Products != null)
            {
                order.Products.Clear();

                foreach (var productDto in updateOrderDTO.Products)
                {
                    var product = _mapper.Map<Product>(productDto);
                    product.OrderId = order.Id;
                    order.Products.Add(product);
                }
            }

            if (order.Products == null || !order.Products.Any())
                throw new Exception("Products list cannot be empty or null.");

            // Recalculate shipping cost and total cost
            order.ShippingCost = await CalculateOrderShippingCost(order);
            if (order.Products == null || !order.Products.Any())
                throw new Exception("Products list cannot be empty or null.");

            order.TotalWeight = order.Products.Sum(p => p.Weight * p.Quantity);
            order.TotalCost = order.ShippingCost + order.Products.Sum(p => p.Price * p.Quantity);

            try
            {
                _unitOfWork.OrderRepo.Update(order);
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("EF Save failed: " + ex.InnerException?.Message, ex);
            }

        }
        public async Task SoftDeleteOrder(int id)
        {
            var order = _unitOfWork.OrderRepo.GetById(id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            order.IsDeleted = true; // Soft delete
            _unitOfWork.OrderRepo.Update(order);
             await _unitOfWork.SaveAsync();
        }
        //public async Task HardDeleteOrder(int id)
        //{
        //    var order = _unitOfWork.OrderRepo.GetById(id);
        //    if (order == null)
        //    {
        //        throw new Exception("Order not found");
        //    }
        //    _unitOfWork.OrderRepo.Delete(order);
        //    _unitOfWork.SaveAsync();

        //}

        public async Task<decimal> CalculateOrderShippingCost (AddOrderDTO orderDTO)
        {
            decimal shippingCost = 0;
            var generalSettings = await _generalSetting.GetGeneralSettingsAsync();
            var OrderCity = _unitOfWork.CityRepo.GetById(orderDTO.CityId) ?? throw new Exception("City not found");
            shippingCost += OrderCity.NormalPrice;
            if (orderDTO.IsPickup)
            {
                var seller = _unitOfWork.SellerRepo.GetById(orderDTO.SellerId) ?? throw new Exception("Seller not found");
                var sellerCity = _unitOfWork.CityRepo.GetById(seller.CityId) ?? throw new Exception("Seller's City not found");
                shippingCost += sellerCity.PickupPrice;
            }
            if (orderDTO.IsShippedToVillage)
            {
                shippingCost += generalSettings.ExtraPriceVillage;
            }
            var totalWeight = orderDTO.Products.Sum(p => p.Weight * p.Quantity);
            if (totalWeight > generalSettings.DefaultWeight)
            {
                var extraWeight = totalWeight - generalSettings.DefaultWeight;
                shippingCost += extraWeight * generalSettings.ExtraPriceKg;
            }
            if (orderDTO.ShippingType == ShippingType.Fast)
            {
                shippingCost += (generalSettings.Fast * shippingCost);
            }
            else if (orderDTO.ShippingType == ShippingType.Express24h)
            {
                shippingCost += (generalSettings.Express * shippingCost);
            }
            return shippingCost;
        }
        public async Task<decimal> CalculateOrderShippingCost(Order orderDTO)
        {
            decimal shippingCost = 0;
            var generalSettings = await _generalSetting.GetGeneralSettingsAsync();
            var OrderCity = _unitOfWork.CityRepo.GetById(orderDTO.CityId) ?? throw new Exception("City not found");
            shippingCost += OrderCity.NormalPrice;
            if (orderDTO.IsPickup)
            {
                var seller = _unitOfWork.SellerRepo.GetById(orderDTO.SellerId) ?? throw new Exception("Seller not found");
                var sellerCity = _unitOfWork.CityRepo.GetById(seller.CityId) ?? throw new Exception("Seller's City not found");
                shippingCost += sellerCity.PickupPrice;
            }
            if (orderDTO.IsShippedToVillage)
            {
                shippingCost += generalSettings.ExtraPriceVillage;
            }
            var totalWeight = orderDTO.Products.Sum(p => p.Weight * p.Quantity);
            if (totalWeight > generalSettings.DefaultWeight)
            {
                var extraWeight = totalWeight - generalSettings.DefaultWeight;
                shippingCost += extraWeight * generalSettings.ExtraPriceKg;
            }
            if (orderDTO.ShippingType == ShippingType.Fast)
            {
                shippingCost += (generalSettings.Fast * shippingCost);
            }
            else if (orderDTO.ShippingType == ShippingType.Express24h)
            {
                shippingCost += (generalSettings.Express * shippingCost);
            }
            return shippingCost;
        }

        //Calculate Order Cost
        public async Task<decimal> CalculateOrderShippingCost(OrderCostDTO order)
        {
            decimal shippingCost = 0;
            var generalSettings = await _generalSetting.GetGeneralSettingsAsync();
            var orderCity = _unitOfWork.CityRepo.GetById(order.OrderCityId) ?? throw new Exception("City not found");
            shippingCost += orderCity.NormalPrice;
            if (order.IsPickup)
            {
                var sellerCity = _unitOfWork.CityRepo.GetById(order.SellerCityId) ?? throw new Exception("Seller's City not found");
                shippingCost += sellerCity.PickupPrice;
            }
            if (order.IsShippedToVillage)
            {
                shippingCost += generalSettings.ExtraPriceVillage;
            }
            if (order.TotalWeight > generalSettings.DefaultWeight)
            {
                var extraWeight = order.TotalWeight - generalSettings.DefaultWeight;
                shippingCost += extraWeight * generalSettings.ExtraPriceKg;
            }
            if (order.ShippingType == ShippingType.Fast)
            {
                shippingCost += (generalSettings.Fast * shippingCost);
            }
            else if (order.ShippingType == ShippingType.Express24h)
            {
                shippingCost += (generalSettings.Express * shippingCost);
            }
            return shippingCost;

        }

        //Change Order Status
        public async Task ChangeOrderStatus(int orderId, OrderStatus newStatus)
        {
            var order = _unitOfWork.OrderRepo.GetById(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            order.Status = newStatus;
            _unitOfWork.OrderRepo.Update(order);
            await _unitOfWork.SaveAsync();
        }

        //Assign delivery agent to order
        public async Task AssignDeliveryAgentToOrder(int orderId, int deliveryAgentId)
        {
            var order = _unitOfWork.OrderRepo.GetById(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            var deliveryAgent = _unitOfWork.DeliveryManRepo.GetById(deliveryAgentId);
            if (deliveryAgent == null || deliveryAgent.User.IsDeleted == true)
            {
                throw new Exception("Delivery agent not found or is deleted");
            }
            order.DeliveryAgentId = deliveryAgentId;
            order.Status = OrderStatus.AcceptedByDeliveryCompany;
            _unitOfWork.OrderRepo.Update(order);
            await _unitOfWork.SaveAsync();
        }
        //Change to user UserID
        //Get Orders By Delivery Agent Id
        public async Task<PagedResponse<ReadOrderDTO>> GetOrdersByDeliveryAgentIdAsync(int deliveryAgentId , PaginationDTO pagination)
        {
            var orders = _unitOfWork.OrderRepo.GetAll()
                .Where(o => o.DeliveryAgentId == deliveryAgentId && o.IsDeleted == false);
            var count = orders.Count();
            var pagedOrders = orders
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            var readOrders = _mapper.Map<List<ReadOrderDTO>>(pagedOrders);
            var result = new PagedResponse<ReadOrderDTO>
            {
                Items = readOrders,
                TotalCount = count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize)
            };
            return result;
        }
        //Change to user UserID
        //Get Orders By Seller Id
        public async Task<PagedResponse<ReadOrderDTO>> GetOrdersBySellerIdAsync(int sellerId , PaginationDTO pagination)
        {
            var orders = _unitOfWork.OrderRepo.GetAll()
                .Where(o => o.SellerId == sellerId && o.IsDeleted == false);
            var count = orders.Count();
            var pagedOrders = orders
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            var readOrders = _mapper.Map<List<ReadOrderDTO>>(pagedOrders);
            var result = new PagedResponse<ReadOrderDTO>
            {
                Items = readOrders,
                TotalCount = count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize)
            };
            return result;
        }

        //Get OrderByStatus
        public async Task<PagedResponse<ReadOrderDTO>> GetOrdersByStatusAsync(OrderStatus status , PaginationDTO pagination)
        {
            var orders = _unitOfWork.OrderRepo.GetAll()
                .Where(o => o.Status == status && o.IsDeleted == false);
            var count = orders.Count();
            var pagedOrders = orders
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            var readOrders = _mapper.Map<List<ReadOrderDTO>>(pagedOrders);
            var result = new PagedResponse<ReadOrderDTO>
            {
                Items = readOrders,
                TotalCount = count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize)
            };
            return result;
        }

        //Get Order Status Count
        public async Task<EnumDTO> GetOrderStatusCount(OrderStatus status)
        {
            var orders = _unitOfWork.OrderRepo.GetAll()
                .Where(o => o.Status == status && o.IsDeleted == false);
            var count = orders.Count();
            return new EnumDTO
            {
                Name = status.ToString(),
                Count = count
            };

        }

        //Get All Order Status Counts
        public async Task<PagedResponse<EnumDTO>> GetAllOrderStatusCounts(PaginationDTO pagination)
        {
            var orderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
            var statusCounts = new List<EnumDTO>();
            foreach (var status in orderStatuses)
            {
                var count = await GetOrderStatusCount(status);
                statusCounts.Add(count);
            }
            // Apply pagination to the status counts
            statusCounts = statusCounts
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            // Create a paged response
            var pagedResponse = new PagedResponse<EnumDTO>
            {
                Items = statusCounts,
                TotalCount = statusCounts.Count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)statusCounts.Count / pagination.PageSize)
            };

            return pagedResponse;
        }

        //Get Order status count fot seller
        public async Task<EnumDTO> GetOrderStatusCountForSeller(int sellerId, OrderStatus status)
        {
            var orders = _unitOfWork.OrderRepo.GetAll()
                .Where(o => o.SellerId == sellerId && o.Status == status && o.IsDeleted == false);
            var count = orders.Count();
            return new EnumDTO
            {
                Name = status.ToString(),
                Count = count
            };
        }

        //Get All Order Status Counts for Seller
        public async Task<PagedResponse<EnumDTO>> GetAllOrderStatusCountsForSeller(int sellerId , PaginationDTO pagination)
        {
            var orderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
            var statusCounts = new List<EnumDTO>();
            foreach (var status in orderStatuses)
            {
                var count = await GetOrderStatusCountForSeller(sellerId, status);
                statusCounts.Add(count);
            }
            // Apply pagination to the status counts
            statusCounts = statusCounts
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            // Create a paged response
            var pagedResponse = new PagedResponse<EnumDTO>
            {
                Items = statusCounts,
                TotalCount = statusCounts.Count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)statusCounts.Count / pagination.PageSize)
            };
            return pagedResponse;
        }

        //Get Order status count fot delivery agent
        public async Task<EnumDTO> GetOrderStatusCountForDeliveryAgent(int deliveryAgentId, OrderStatus status)
        {
            var orders = _unitOfWork.OrderRepo.GetAll()
                .Where(o => o.DeliveryAgentId == deliveryAgentId && o.Status == status && o.IsDeleted == false);
            var count = orders.Count();
            return new EnumDTO
            {
                Name = status.ToString(),
                Count = count
            };
        }
        //Get All Order Status Counts for Delivery Agent
        public async Task<PagedResponse<EnumDTO>> GetAllOrderStatusCountsForDeliveryAgent(int deliveryAgentId , PaginationDTO pagination)
        {
            var orderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
            var statusCounts = new List<EnumDTO>();
            foreach (var status in orderStatuses)
            {
                var count = await GetOrderStatusCountForDeliveryAgent(deliveryAgentId, status);
                statusCounts.Add(count);
            }
            // Apply pagination to the status counts
            statusCounts = statusCounts
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            // Create a paged response
            var pagedResponse = new PagedResponse<EnumDTO>
            {
                Items = statusCounts,
                TotalCount = statusCounts.Count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)statusCounts.Count / pagination.PageSize)
            };
            return pagedResponse;
        }

        //Get Shipping types using ShippingAndOrderAndPaymentTypeDTO
        public async Task<List<ShippingAndOrderAndPaymentTypeDTO>> GetShippingTypesAsync()
        {
            var shippingTypes = Enum.GetValues(typeof(ShippingType)).Cast<ShippingType>();
            return shippingTypes.Select(st => new ShippingAndOrderAndPaymentTypeDTO
            {
                Name = st.ToString(),
                Id = (int)st
            }).ToList();
        }

        public async Task<List<ShippingAndOrderAndPaymentTypeDTO>> GetOrdersTypesAsync()
        {
            var shippingTypes = Enum.GetValues(typeof(OrderType)).Cast<OrderType>();
            return shippingTypes.Select(st => new ShippingAndOrderAndPaymentTypeDTO
            {
                Name = st.ToString(),
                Id = (int)st
            }).ToList();
        }

        public async Task<List<ShippingAndOrderAndPaymentTypeDTO>> GetPaymentTypesAsync()
        {
            var shippingTypes = Enum.GetValues(typeof(PaymentType)).Cast<PaymentType>();
            return shippingTypes.Select(st => new ShippingAndOrderAndPaymentTypeDTO
            {
                Name = st.ToString(),
                Id = (int)st
            }).ToList();
        }



    }
}
