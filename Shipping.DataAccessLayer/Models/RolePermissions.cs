using Microsoft.AspNetCore.Identity;
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
    public class RolePermissions
    {
        public int Id { get; set; }

        //[ForeignKey("Role")]
        [Required]
        public string RoleName { get; set; }
        public Department Department { get; set; }
        public bool View { get; set; } = true;
        public bool Add { get; set; } = true;
        public bool Edit { get; set; } = true;
        public bool Delete { get; set; } = true;
        public virtual IdentityRole Role { get; set; }

    }
}
