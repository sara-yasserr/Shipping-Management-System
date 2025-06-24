using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1;  // Default to 1
        [Required(ErrorMessage = "Product weight is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Weight must be a positive number")]
        public decimal Weight { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }


        // Navigation property
        public virtual Order? Order { get; set; }

    }
}
