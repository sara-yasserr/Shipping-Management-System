using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.DataAccessLayer.Models;

namespace Shipping.BusinessLogicLayer.DTOs.BranchDTOs
{
    public class AddBranch
    {
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int CityId { get; set; }
    }
}
