using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Enum
{
    public enum PaymentType
    {
        [Display(Name = "Cash on Delivery")]
        CashOnDelivery = 1,  // واجبة التحصيل

        [Display(Name = "Prepaid")]
        Prepaid,         // دفع مقدم

        [Display(Name = "Parcel Exchange")]
        ParcelExchange   // طرد مقابل طرد
    }
}
