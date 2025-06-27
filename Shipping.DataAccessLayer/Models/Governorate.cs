using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class Governorate
    {
       public int Id { get; set; }
        [Required(ErrorMessage = "Governorate name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Governorate name must be between 2 and 100 characters")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;

        //Navigaion
        public virtual List<City> Cities { get; set; } = new List<City>();
    }
}
