using Shipping.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Default status

        [StringLength(500)]
        public string? Notes { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalWeight { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string CustomerPhone { get; set; }

        public bool IsShippedToVillage { get; set; } = false;

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public ShippingType ShippingType { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public bool IsPickup { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        [Range(0, int.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ShippingCost { get; set; }

        [Range(0, int.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCost { get; set; }
        // Foreign Keys
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        [ForeignKey("DeliveryAgent")]
        public int DeliveryAgentId { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }

        [ForeignKey("Branch")]
        public int BranchId { get; set; }

        // Navigation Properties
        public virtual Seller Seller { get; set; }
        public virtual DeliveryAgent? DeliveryAgent { get; set; }
        public virtual City City { get; set; }
        public virtual Branch? Branch { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();
    }

}
