using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class DeliveryAgentCities
    {
        public int Id { get; set; }
        // Foreign Keys
        [ForeignKey("DeliveryAgent")]
        public int DeliveryAgentId { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        // Navigation Properties
        public virtual DeliveryAgent DeliveryAgent { get; set; }
        public virtual City City { get; set; }

    }
}
