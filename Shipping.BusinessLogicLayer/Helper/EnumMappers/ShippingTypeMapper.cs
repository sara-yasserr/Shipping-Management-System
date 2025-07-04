using Shipping.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Helper.EnumMappers
{
    public static class ShippingTypeMapper
    {
        public static Dictionary<ShippingType, string> ShippingTypeDictionary = new Dictionary<ShippingType, string>()
        {
            { ShippingType.Standard , "Standard" },
            { ShippingType.Fast, "Fast" },
            { ShippingType.Express24h, "Express" }
        };
        public static string GetShippingTypeName(ShippingType shippingType)
        {
            return ShippingTypeDictionary.TryGetValue(shippingType, out var name) ? name : "Unknown Shipping Type";
        }
    }
}
