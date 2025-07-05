using Shipping.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Helper.EnumMappers
{
    public static class OrderTypeMapper
    {
        public static Dictionary<OrderType, string> OrderTypeDictionary = new Dictionary<OrderType, string>()
        {
            { OrderType.Normal, "Normal" },
            { OrderType.Pickup, "Pickup"}
        };
        public static string GetOrderTypeName(OrderType orderType)
        {
            return OrderTypeDictionary.TryGetValue(orderType, out var name) ? name : "Unknown Order Type";
        }
    }
}
