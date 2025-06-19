using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class Seller
    {
        [key]
        public int Id { get; set; }
        [Required]
        public string StoreName { get; set; }
        [Required]
        public string Address { get; set; }
        public decimal CancelledOrderPercentage { get; set; }
        //navigation property
        public virtual List<Order> Orders { get; set; }
    }
}
