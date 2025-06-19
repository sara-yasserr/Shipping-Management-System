using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class City
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal NormalPrice { get; set; }
        public decimal PickupPrice { get; set; }
        // Foreign Keys
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public virtual Governorate Governorate { get; set; }
        // Navigation Properties
        public virtual List<Branch>? Branches { get; set; }
        public virtual List<DeliveryAgent>? DeliveryAgents { get; set; }

    }
}
