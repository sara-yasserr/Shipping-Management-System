using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.BranchDTOs
{
    public class ReadBranch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string City { get; set; }

        public bool? IsDeleted { get; set; }
        public List<string>? DeliverAgents { get; set; }
        public List<string>? Employees { get; set; }


    }
}
