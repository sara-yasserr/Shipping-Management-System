using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Enum
{
    public enum OrderType
    {
        [Display(Name = "Normal")]
        Normal = 1,
        [Display(Name = "Pickup")]
        Pickup
    }
}
