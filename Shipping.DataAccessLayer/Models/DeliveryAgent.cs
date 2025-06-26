using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class DeliveryAgent
    {
        [Key]
        public int Id { get; set; }
        // Foreign Keys
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        // Navigation Properties
        public virtual List<Order> Orders { get; set; } = new List<Order>();
        public virtual List<DeliveryAgentCities> DeliveryAgentCities { get; set; } = new List<DeliveryAgentCities>();
    }
}
