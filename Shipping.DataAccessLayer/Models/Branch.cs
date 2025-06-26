using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual Governorate Governorate { get; set; }
        public virtual List<DeliveryAgent> DeliveryAgents { get; set; } = new List<DeliveryAgent>();
        public virtual List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
