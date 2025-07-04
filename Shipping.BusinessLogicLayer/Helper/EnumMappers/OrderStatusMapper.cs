using Shipping.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Helper.EnumMappers
{
    public static class OrderStatusMapper
    {
        public static Dictionary<OrderStatus, string> OrderStatusDictionary = new Dictionary<OrderStatus, string>()
        {
            { OrderStatus.Pending, "Pending" },
            { OrderStatus.AcceptedByDeliveryCompany, "Accepted By Company" },
            { OrderStatus.RejectedByDeliveryCompany, "Rejected By Company" },
            { OrderStatus.Delivered, "Delivered" },
            { OrderStatus.DeliveredToDeliveryMan, "Delivered To Delivery Man" },
            { OrderStatus.CanNotBeReached, "Cannot Be Reached" },
            { OrderStatus.Postponed, "Postponed" },
            { OrderStatus.PartiallyDelivered, "Partially Delivered" },
            { OrderStatus.CanceledByCustomer, "Canceled By Customer" },
            { OrderStatus.RejectWithPayment, "Rejected With Payment" },
            { OrderStatus.RejectWithoutPayment, "Rejected Without Payment" },
            { OrderStatus.RejectWithPartiallyPaid, "Rejected With Partial Payment" }
        };
        public static string GetOrderStatusName(OrderStatus status)
        {
            return OrderStatusDictionary.TryGetValue(status, out var name) ? name : "Unknown Status";
        }
    }
}
