using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs
{
    public class AddEmployeeDTO
    {
        public int? BranchId { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public string? Password { get; set; }
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; } = "Admin";
    }
}
