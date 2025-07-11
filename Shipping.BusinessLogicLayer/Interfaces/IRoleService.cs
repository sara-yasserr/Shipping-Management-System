using Microsoft.AspNetCore.Identity;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IRoleService
    {
        List<string?> GetAllRoles();
        PagedResponse<string> GetAllPaginated(PaginationDTO pagination);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityRole?> GetRoleByNameAsync(string roleName);
        Task<bool> UpdateRole(string oldRoleName, string newRoleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<List<PermissionDTO>> GetPermissionsForRoleAsync(string roleName);
        Task<bool> UpdatePermissionsAsync(string roleName, List<PermissionDTO> permissions);
    }
}
