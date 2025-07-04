using Microsoft.AspNetCore.Identity;
using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;
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
        Task<List<string>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityRole?> GetRoleByNameAsync(string roleName);
        Task<bool> UpdateRole(string oldRoleName, string newRoleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);
        Task<List<PermissionDTO>> GetPermissionsForRoleAsync(string roleName);
        Task<bool> UpdatePermissionsAsync(string roleName, List<PermissionDTO> permissions);
    }
}
