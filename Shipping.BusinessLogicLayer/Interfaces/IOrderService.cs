using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.EnumDTOs;
using Shipping.BusinessLogicLayer.DTOs.OrderDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IOrderService
    {
        Task<PagedResponse<ReadOrderDTO>> GetAllOrdersAsync(PaginationDTO pagination); //
        Task<List<ReadOrderDTO>> GetAllWithoutPagination();//
        Task<ReadOneOrderDTO> GetOrderById(int id); //
        Task AddOrder(AddOrderDTO orderDTO); //
        Task UpdateOrder(UpdateOrderDTO updateOrderDTO); //
        Task SoftDeleteOrder(int id); //
        //Task HardDeleteOrder(int id);
        Task ChangeOrderStatus(int orderId, OrderStatus newStatus); //
        Task AssignDeliveryAgentToOrder(int orderId, int deliveryAgentId); //
        Task<decimal> CalculateOrderShippingCost(OrderCostDTO orderDTO); //
        Task<PagedResponse<ReadOrderDTO>> GetOrdersByDeliveryAgentIdAsync(int deliveryAgentId, PaginationDTO pagination); //
        Task<PagedResponse<ReadOrderDTO>> GetOrdersBySellerIdAsync(int sellerId, PaginationDTO pagination); //
        Task<PagedResponse<ReadOrderDTO>> GetOrdersByStatusAsync(OrderStatus status, PaginationDTO pagination); //
        Task<EnumDTO> GetOrderStatusCount(OrderStatus status); //
        Task<PagedResponse<EnumDTO>> GetAllOrderStatusCounts(PaginationDTO pagination); //
        Task<EnumDTO> GetOrderStatusCountForSeller(int sellerId, OrderStatus status); //
        Task<PagedResponse<EnumDTO>> GetAllOrderStatusCountsForSeller(int sellerId, PaginationDTO pagination); //
        Task<EnumDTO> GetOrderStatusCountForDeliveryAgent(int deliveryAgentId, OrderStatus status); //
        Task<PagedResponse<EnumDTO>> GetAllOrderStatusCountsForDeliveryAgent(int deliveryAgentId, PaginationDTO pagination); //
        Task<List<ShippingAndOrderAndPaymentTypeDTO>> GetShippingTypesAsync(); //
        Task<List<ShippingAndOrderAndPaymentTypeDTO>> GetOrdersTypesAsync(); //
        Task<List<ShippingAndOrderAndPaymentTypeDTO>> GetPaymentTypesAsync(); //

    }
}
