using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StoreName { get; set; }
        [Required]
        public string Address { get; set; }
        public decimal CancelledOrderPercentage { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        //navigation property
        public virtual City City { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}
