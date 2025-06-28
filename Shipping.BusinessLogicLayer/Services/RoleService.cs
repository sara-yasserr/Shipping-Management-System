using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;
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
        private IMapper _mapper;
        public RoleService(UnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public Task<bool> UpdateRole(string oldRoleName, string newRoleName)
        {
            var role = _unitOfWork._roleManager.FindByNameAsync(oldRoleName);
            if (role == null)
            {
                throw new ArgumentException("Role not found.", nameof(oldRoleName));
            }
            role.Result.Name = newRoleName;
            role.Result.NormalizedName = _unitOfWork._roleManager.NormalizeKey(newRoleName);
            var result = _unitOfWork._roleManager.UpdateAsync(role.Result);
            if (result.Result.Succeeded)
            {
                return Task.FromResult(true);
            }
            else
            {
                throw new Exception("Failed to update role: " + string.Join(", ", result.Result.Errors.Select(e => e.Description)));
            }
        }
        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await GetRoleByNameAsync(roleName);
            if (role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            return await _unitOfWork._roleManager.DeleteAsync(role);
        }
        public async Task<List<PermissionDTO>> GetPermissionsForRoleAsync(string roleName)
        {
            var permissions =  _unitOfWork.RolePermissionsRepo
                .GetAll()
                .Where(p => p.RoleName == roleName)
                .ToList();
            return _mapper.Map<List<RolePermissions>, List<PermissionDTO>>(permissions);
        }
        public bool IsRolePermissionExist(string role)
        {
            return _unitOfWork.RolePermissionsRepo
                .GetAll()
                .Any(p => p.RoleName == role);
        }
        public async Task<bool> UpdatePermissionsAsync(string roleName, List<PermissionDTO> permissions)
        {
            var oldPermissions = _unitOfWork.RolePermissionsRepo
                .GetAll()
                .Where(p => p.RoleName == roleName)
                .ToList();

            var newPermissions = _mapper.Map<List<PermissionDTO>, List<RolePermissions>>(permissions);
            if (!IsRolePermissionExist(roleName) ) return false;
            if (newPermissions == null || !newPermissions.Any())
            {
                throw new ArgumentException("Permissions cannot be null or empty.", nameof(permissions));
            }
            foreach (var perm in newPermissions)
            {
                perm.RoleName = roleName;
                _unitOfWork.RolePermissionsRepo.Update(perm);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }

        
    }
}
