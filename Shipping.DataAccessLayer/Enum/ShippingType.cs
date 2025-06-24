using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Enum
{
    public enum ShippingType
    {
        [Display(Name = "Standard (5-7 days)")]
        Standard = 1,

        [Display(Name = "Express (24h)")]
        Express24h,

        [Display(Name = "Economy (15 days)")]
        Economy15d,

        [Display(Name = "Priority (89h)")] //3-4 days
        Priority89h
    }
}
