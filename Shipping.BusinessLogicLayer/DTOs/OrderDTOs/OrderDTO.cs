using Shipping.DataAccessLayer.Enum;
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
}
