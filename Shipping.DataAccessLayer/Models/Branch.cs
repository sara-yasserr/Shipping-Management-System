using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class Branch
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
        public int GovernorateId { get; set; }
        //Navigation
        [ForeignKey("GovernrateId")]
        public Governorate Governorate { get; set; }
        public virtual List<DeliveryAgent> DeliveryAgents { get; set; } = new List<DeliveryAgent>();
        public virtual List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
