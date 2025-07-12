using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.BusinessLogicLayer.DTOs.City;
using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.BusinessLogicLayer.DTOs.GeneralSettingsDTOs;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;

using Shipping.BusinessLogicLayer.DTOs.Seller;

using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;

using Shipping.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.BusinessLogicLayer.DTOs.OrderDTOs;
using Shipping.DataAccessLayer.Enum;
using Shipping.BusinessLogicLayer.Helper.EnumMappers;

namespace Shipping.BusinessLogicLayer.Helper
{
    public class MappConfig : Profile
    {
        public MappConfig()
        {
            #region Branch
            CreateMap<Branch, ReadBranch>()
               .AfterMap((src, dest) =>
               {
                   dest.City = src.City.Name;

                   dest.DeliverAgents = src.DeliveryAgents.Select(d => d.User.UserName).ToList();
                   dest.Employees = src.Employees.Select(e => e.User.UserName).ToList();
               }).ReverseMap();

            CreateMap<AddBranch, Branch>().AfterMap((src, dest) =>
            {
                dest.CreationDate = DateTime.Now;
            });
            #endregion


            #region seller

            CreateMap<Seller, SellerDTO>().AfterMap((src, dest) =>
            {
                dest.CityName = src.City.Name;
                dest.Username = src.User.UserName;
                dest.FullName = src.User.FirstName + " " + src.User.LastName;
                dest.Email = src.User.Email;
                dest.PhoneNumber = src.User.PhoneNumber;
                dest.UserId = src.UserId;
            }).ReverseMap();



            CreateMap<AddSellerDTO, Seller>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Seller, AddSellerDTO>();

            CreateMap<Seller, UpdateSellerDTO>().ReverseMap();


            #endregion


            #region Governorate
            CreateMap<Governorate, ReadGovernorateDto>();
            CreateMap<AddGovernorateDto, Governorate>();
            #endregion

            #region Employee
            CreateMap<Employee, ReadEmployeeDTO>().AfterMap((src, dest) =>
            {
                if (src.Branch != null)
                {
                    dest.Branch = src.Branch.Name;
                }
                else
                {
                    dest.Branch = "General";
                }
                dest.Id = src.Id;
                dest.UserName = src.User.UserName;
                dest.Email = src.User.Email;
                dest.FirstName = src.User.FirstName;
                dest.LastName = src.User.LastName;
                dest.PhoneNumber = src.User.PhoneNumber;
                dest.CreatedAt = src.User.CreatedAt;
                dest.IsDeleted = src.User.IsDeleted;
                dest.Password = src.User.PasswordHash;
            });

            CreateMap<AddEmployeeDTO, Employee>().AfterMap((src, dest) =>
            {
                dest.BranchId = src.BranchId;

                dest.User = new ApplicationUser
                {
                    UserName = src.UserName,
                    Email = src.Email,
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    PhoneNumber = src.PhoneNumber
                };
            });
            #endregion

            #region City
            CreateMap<CreateCityDTO, City>();
            #endregion

            #region General Settings
            CreateMap<GeneralSetting, ReadGeneralSettingsDTO>().ReverseMap();
            CreateMap<UpdateGeneralSettingsDTO, GeneralSetting>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => 1))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.Fast, opt => opt.MapFrom(src => (decimal)src.Fast))
            .ForMember(dest => dest.Express, opt => opt.MapFrom(src => (decimal)src.Express))
            .ForMember(dest => dest.DefaultWeight, opt => opt.MapFrom(src => (decimal)src.DefaultWeight))
            .ForMember(dest => dest.ExtraPriceKg, opt => opt.MapFrom(src => (decimal)src.ExtraPriceKg))
            .ForMember(dest => dest.ExtraPriceVillage, opt => opt.MapFrom(src => (decimal)src.ExtraPriceVillage))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId)).ReverseMap();

            #endregion


            #region Role Permissions
            CreateMap<RolePermissions, PermissionDTO>()
            .ForMember(dest => dest.DepartmentName,
                       opt => opt.MapFrom(src => DepartmentMapper.GetDepartmentName(src.Department)))
            .ReverseMap()
            .ForMember(dest => dest.Department,
                       opt => opt.MapFrom(src => src.Department));

            #endregion

            #region DeliveryMan
            CreateMap<DeliveryAgent, ReadDeliveryMan>()
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                   src.User != null ? src.User.FirstName + " " + src.User.LastName : null))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
               .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.User.IsDeleted))
               .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
               .ForMember(dest => dest.Cities, opt => opt.MapFrom(src =>
                   src.Cities != null ? string.Join(", ", src.Cities.Select(c => c.Name)) : null))

               .ForMember(dest => dest.CityIds, opt => opt.MapFrom(src =>
                   src.Cities != null ? src.Cities.Select(c => c.Id).ToList() : null))
               .ForMember(dest => dest.ActiveOrdersCount, opt => opt.MapFrom(src =>
                   src.Orders != null ? src.Orders.Count(o => o.IsActive) : 0))
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.User.IsDeleted))
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
            CreateMap<AddDeliveryMan, DeliveryAgent>().AfterMap((src, dest) =>
            {
                dest.BranchId = src.BranchId;

                dest.User = new ApplicationUser
                {
                    UserName = src.UserName,
                    Email = src.Email,
                    FirstName = src.Name?.Split(' ').FirstOrDefault() ?? src.Name,
                    LastName = src.Name?.Contains(' ') == true ? src.Name.Substring(src.Name.IndexOf(' ') + 1) : string.Empty,
                    PhoneNumber = src.PhoneNumber
                };

            });

            CreateMap<UpdateDeliveryMan, DeliveryAgent>().AfterMap((src, dest) =>
            {
                dest.BranchId = src.BranchId;

                if (dest.User != null)
                {
                    dest.User.UserName = src.UserName;
                    dest.User.Email = src.Email;
                    dest.User.PhoneNumber = src.PhoneNumber;
                    dest.User.FirstName = src.Name?.Split(' ').FirstOrDefault() ?? src.Name;
                    dest.User.LastName = src.Name?.Contains(' ') == true ? src.Name.Substring(src.Name.IndexOf(' ') + 1) : string.Empty;
                    dest.User.IsDeleted = src.IsDeleted;
                }
            });
            #endregion


            #region Orders

            CreateMap<AddOrderDTO, Order>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => OrderStatus.Pending))
            .ForMember(dest => dest.TotalWeight, opt => opt.Ignore())
            .ForMember(dest => dest.TotalCost, opt => opt.Ignore())
            .ForMember(dest => dest.ShippingCost, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DeliveryAgentId, opt => opt.MapFrom(_ => (int?)null));

            //Add Product
            CreateMap<AddProductDTO, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore());

            // Order → ReadOrderDTO
            CreateMap<Order, ReadOrderDTO>()
            .ConstructUsing(src => new ReadOrderDTO(
                src.Id,
                src.Notes,
                src.CustomerName,
                src.CustomerPhone,
                src.City.Name,
                src.Seller != null && src.Seller.User != null ? $"{src.Seller.User.FirstName} {src.Seller.User.LastName}" : null,
                src.Seller.City.Name,
                src.DeliveryAgent != null && src.DeliveryAgent.User != null ? $"{src.DeliveryAgent.User.FirstName} {src.DeliveryAgent.User.LastName}" : null,
                src.Branch.Name,
                src.IsShippedToVillage,
                src.Address,
                src.CreationDate,
                src.Status.ToString(),
                src.ShippingType.ToString(),
                src.OrderType.ToString(),
                src.PaymentType.ToString(),
                src.IsPickup,
                src.IsActive,
                src.IsDeleted,
                src.ShippingCost,
                src.TotalCost,
                src.TotalWeight
            ));

            CreateMap<Order, ReadOneOrderDTO>()
    .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.CustomerCityName, opt => opt.MapFrom(src => src.City.Name))
    .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller != null && src.Seller.User != null
        ? $"{src.Seller.User.FirstName} {src.Seller.User.LastName}" : null))
    .ForMember(dest => dest.SellerCityName, opt => opt.MapFrom(src => src.Seller.City.Name))
    .ForMember(dest => dest.DeliveryAgentName, opt => opt.MapFrom(src => src.DeliveryAgent != null && src.DeliveryAgent.User != null
        ? $"{src.DeliveryAgent.User.FirstName} {src.DeliveryAgent.User.LastName}" : null))
    .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
    .ForMember(dest => dest.ShippingType, opt => opt.MapFrom(src => src.ShippingType.ToString()))
    .ForMember(dest => dest.OrderType, opt => opt.MapFrom(src => src.OrderType.ToString()))
    .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType.ToString()))
    .ForMember(dest => dest.Products, opt => opt.MapFrom(src =>
        src.Products.Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Weight = p.Weight,
            Quantity = p.Quantity,
            OrderId = p.OrderId
        }).ToList()));






            // Product → ProductDTO
            CreateMap<ProductDTO, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())           // تجاهل Id في Product
            .ForMember(dest => dest.OrderId, opt => opt.Ignore())      // تجاهل OrderId
            .ForMember(dest => dest.Order, opt => opt.Ignore());       // تجاهل الـ navigation


            // Product → ReadProductDTO
            CreateMap<Product, ReadProductDTO>();

            CreateMap<UpdateOrderDTO, Order>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.DeliveryAgentId, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));



            #endregion
        }
    }
}
