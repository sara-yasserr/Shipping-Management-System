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
        [Display(Name = "Standard (5-7 days)")] //100%
        Standard = 1,


        [Display(Name = "Priority (3 - 4 Days)")] //3-4 days //120%
        Fast,

        [Display(Name = "Express (24 H)")] //150%
        Express24h
    }
}
