using Shipping.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Helper.EnumMappers
{
    public static class PaymentTypeMapper
    {
        public static Dictionary<PaymentType , string> PaymentTypeDictionary = new Dictionary<PaymentType, string>()
        {
            { PaymentType.CashOnDelivery, "Cash on Delivery" },
            { PaymentType.ParcelExchange, "Online Payment" },
            { PaymentType.Prepaid, "Bank Transfer" }
        };

        public static string GetPaymentTypeName(PaymentType paymentType)
        {
            return PaymentTypeDictionary.TryGetValue(paymentType, out var name) ? name : "Unknown Payment Type";
        }
    }
}
