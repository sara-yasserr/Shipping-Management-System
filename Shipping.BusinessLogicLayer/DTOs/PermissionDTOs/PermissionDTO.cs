using Shipping.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.PermissionDTOs
{
    public class PermissionDTO
    {
        public Department Department { get; set; }
        public string DepartmentName { get; set; }
        public bool View { get; set; } = true;
        public bool Add { get; set; } = true;
        public bool Edit { get; set; } = true;
        public bool Delete { get; set; } = true;
    }
}
