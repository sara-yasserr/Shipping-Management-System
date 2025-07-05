using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AddGovernorateDto
{
    [Required(ErrorMessage = "Governorate name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Governorate name must be between 2 and 100 characters")]
    public string Name { get; set; }
}
