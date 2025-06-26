using Microsoft.AspNetCore.Identity;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Services
{
    public class RoleService : IRoleService
    {
        private UnitOfWork _unitOfWork;
        public RoleService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<string>> GetAllRolesAsync()
        {
            return _unitOfWork._roleManager.Roles.Select(r => r.Name).ToList();
        }
        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if ( await RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });

            }
            return await _unitOfWork._roleManager.CreateAsync(new IdentityRole(roleName));
        }
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _unitOfWork._roleManager.RoleExistsAsync(roleName);
        }
        public async Task<IdentityRole?> GetRoleByNameAsync(string roleName)
        {
            return await _unitOfWork._roleManager.FindByNameAsync(roleName);
        }
        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await GetRoleByNameAsync(roleName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            return await _unitOfWork._roleManager.DeleteAsync(role);
        }
        public async Task<List<RolePermissions>> GetPermissionsForRoleAsync(string roleName)
        {
            return _unitOfWork.RolePermissionsRepo
                .GetAll()
                .Where(p => p.RoleName == roleName)
                .ToList();
        }
        public async Task<bool> UpdatePermissionsAsync(string roleName, List<RolePermissions> permissions)
        {
            var oldPermissions = _unitOfWork.RolePermissionsRepo
                .GetAll()
                .Where(p => p.RoleName == roleName)
                .ToList();

            foreach (var perm in oldPermissions)
                _unitOfWork.RolePermissionsRepo.Delete(perm);

            foreach (var perm in permissions)
                _unitOfWork.RolePermissionsRepo.Add(perm);

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
