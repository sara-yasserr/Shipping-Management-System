using Microsoft.AspNetCore.Identity;
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
        Task<IdentityResult> DeleteRoleAsync(string roleName);

        Task<List<RolePermissions>> GetPermissionsForRoleAsync(string roleName);
        Task<bool> UpdatePermissionsAsync(string roleName, List<RolePermissions> permissions);
    }
}
