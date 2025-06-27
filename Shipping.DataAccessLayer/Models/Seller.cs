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
        [Range(0.0, 1.0, ErrorMessage = "Cancelled order percentage must be between 0.0 and 1.0.")]
        [Column(TypeName = "decimal(4, 2)")]
        public decimal CancelledOrderPercentage { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        //navigation property
        public virtual City City { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
