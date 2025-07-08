using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.DataAccessLayer.Models;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IBranchService
    {
        public PagedResponse<ReadBranch> GetAllBranch( PaginationDTO pagination);
        public List<ReadBranch> GetAllBranch();
        public Branch? GetBranchById(int id);
        public ReadBranch? GetBranchDTOById(int id);
        public Branch AddBranch(AddBranch addBranchDTO);
        public void UpdateBranch(AddBranch updateBranchDTO, Branch branch);
        public void SoftDelete(Branch branch);
        public void HardDelete(Branch branch);
        public void ActivateBranch(int branchId);
    }
}
