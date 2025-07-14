using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.OrderDTOs
{
    //public record OrderDTO
    //(
    //    string? notes,
    //    string customerName,
    //    string customerPhone,
    //    bool isShippedToVillage,
    //    string address,
    //    DateTime creationDate,
    //    string status,
    //    ShippingType shippingType,
    //    OrderType orderType,
    //    PaymentType paymentType,
    //    bool isPickup,
    //    bool isActive,
    //    bool isDeleted,
    //    decimal shippingCost,
    //    decimal totalCost, //Shipping cost + product cost
    //    decimal totalWeight,
    //    Lazy<ProductDTO> ProductDTO,
    //    int? deliveryManId = null,
    //    int? sellerId = null,
    //    int? branchId = null,
    //    int? CityId = null
    //);

    public record ReadOrderDTO
    (
        int OrderID,
        string? Notes,
        string CustomerName,
        string CustomerPhone,
        string? CustomerCityName,
        string? SellerName,
        string? SellerCityName,
        string? DeliveryAgentName,
        string? BranchName,
        bool isShippedToVillage,
        string Address,
        DateTime CreationDate,
        String Status,
        string ShippingType,
        string OrderType,
        string PaymentType,
        bool IsPickup,
        bool IsActive,
        bool IsDeleted,
        decimal ShippingCost,
        decimal TotalCost, //Shipping cost + product cost
        decimal TotalWeight

    );
    public class ReadOneOrderDTO
    {
        public int OrderID { get; set; }
        public string? Notes { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string? CustomerCityName { get; set; }
        public string? SellerName { get; set; }
        public string? SellerCityName { get; set; }
        public int? deliveryManId { get; set; }
        public string? DeliveryAgentName { get; set; }
        public string? BranchName { get; set; }
        public bool isShippedToVillage { get; set; }
        public string Address { get; set; }
        public DateTime CreationDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int ShippingTypeID { get; set; }

        public string ShippingType { get; set; }

        public int OrderTypeId { get; set; }

        public string OrderType { get; set; }
        public int PaymentTypeId { get; set; }

        public string PaymentType { get; set; }
        public bool IsPickup { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalWeight { get; set; }
        public List<Product> Products { get; set; }

        public ReadOneOrderDTO() { } // ضروري
    }

    public record AddOrderDTO
    (
        string? Notes,
        string CustomerName,
        string CustomerPhone,
        bool IsShippedToVillage,
        string Address,
        ShippingType ShippingType,
        OrderType OrderType,
        PaymentType PaymentType,
        bool IsPickup,
        int CityId,
        int SellerId,
        int BranchId,
        List<AddProductDTO> Products
    );

    public record UpdateOrderDTO
    (
        int Id,
        string? Notes,
        string? CustomerName,
        string? CustomerPhone,
        bool? IsShippedToVillage,
        string? Address,
        OrderStatus? Status,
        ShippingType? ShippingType,
        OrderType? OrderType,
        PaymentType? PaymentType,
        int CityId,
        int BranchId,
        bool? IsPickup,
        bool? IsActive,
        int? DeliveryManId,
        List<ProductDTO>? Products
    );


    public record OrderCostDTO
    (
        decimal TotalWeight,
        ShippingType ShippingType,
        bool IsPickup,
        bool IsShippedToVillage,
        int OrderCityId,
        int SellerCityId
    );

    public class ChangeOrderStatusDto
    {
        public OrderStatus NewStatus { get; set; }
    }
}
