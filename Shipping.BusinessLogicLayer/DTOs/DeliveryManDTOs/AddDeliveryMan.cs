using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs
{
    public class AddDeliveryMan
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-={}:;<>.,?]).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Phone number must be a valid Egyptian number (e.g., 010xxxxxxxx, 011xxxxxxxx, 012xxxxxxxx, 015xxxxxxxx).")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Branch is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Branch ID must be a positive number.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "At least one city must be selected.")]
        [MinLength(1, ErrorMessage = "At least one city must be selected.")]
        public List<int> CityIds { get; set; }
    }
} 