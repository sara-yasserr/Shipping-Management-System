using Shipping.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Branch")]
        public int? BranchId { get; set; }
        //[ForeignKey("GeneralSetting")]
        //public int? GeneralSettingId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        //Navigation Properties
        public virtual Branch? Branch { get; set; }
        //public virtual GeneralSetting? GeneralSetting { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}

