using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public enum ShippingType
    {
        [Display(Name = "Standard (5-7 days)")]
        Standard,

        [Display(Name = "Express (24h)")]
        Express24h,

        [Display(Name = "Economy (15 days)")]
        Economy15d,

        [Display(Name = "Priority (89h)")] //3-4 days
        Priority89h
    }
    public enum PaymentType
    {
        [Display(Name = "Cash on Delivery")]
        CashOnDelivery,  // واجبة التحصيل

        [Display(Name = "Prepaid")]
        Prepaid,         // دفع مقدم

        [Display(Name = "Parcel Exchange")]
        ParcelExchange   // طرد مقابل طرد
    }
    public enum OrderType
    {
        [Display(Name = "Normal")]
        Normal,
        [Display(Name = "Pickup")]
        Pickup 
    }
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

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        [Range(0, int.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public int TotalCost { get; set; }

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
