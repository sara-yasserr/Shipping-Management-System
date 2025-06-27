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
        public bool IsDeleted { get; set; } = false;
        public DateTime CreationDate { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        //Navigation
        public virtual City City { get; set; }
        public virtual List<DeliveryAgent> DeliveryAgents { get; set; } = new List<DeliveryAgent>();
        public virtual List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
