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
        //Foreign keys
        [ForeignKey("Admin")]
        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
