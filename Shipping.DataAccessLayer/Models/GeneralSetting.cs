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
    public class GeneralSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } = 1; // Default value for the GeneralSetting
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DefaultWeight { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ExtraPriceKg { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ExtraPriceVillage { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        [Range(0.0, 1.0, ErrorMessage = "The value must be between 0.0 and 1.0.")]
        [Column(TypeName = "decimal(4, 2)")]
        public decimal Fast { get; set; } = 0.2M;
        [Column(TypeName = "decimal(4, 2)")]
        [Range(0.0, 1.0, ErrorMessage = "The value must be between 0.0 and 1.0.")]
        public decimal Express { get; set; } = 0.5M;
        //Foreign keys
        public int EmployeeId { get; set; }

        //Navigation properties
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }
}
