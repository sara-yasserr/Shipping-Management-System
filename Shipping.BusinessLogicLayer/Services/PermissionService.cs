using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;

namespace Shipping.BusinessLogicLayer.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly ShippingDBContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        IMapper mapper;


        public PermissionService(ShippingDBContext context, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            _roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task<bool> HasPermissionAsync(string roleName, Department department, PermissionType permissionType)
        {
            var permission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleName == roleName && rp.Department == department);

            if (permission == null) return false;

            return permissionType switch
            {
                PermissionType.View => permission.View,
                PermissionType.Add => permission.Add,
                PermissionType.Edit => permission.Edit,
                PermissionType.Delete => permission.Delete,
                _ => false
            };
        }
        public async Task<bool> HasGeneralPermissionAsync(string roleName, PermissionType permissionType)
        {
            var permission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleName == roleName && rp.Department == null);

            if (permission == null) return false;

            return permissionType switch
            {
                PermissionType.View => permission.View,
                PermissionType.Add => permission.Add,
                PermissionType.Edit => permission.Edit,
                PermissionType.Delete => permission.Delete,
                _ => false
            };
        }

        public async Task UpdateRolePermissionsAsync(PermissionDTO permission)
        {
            var existing = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleName == permission.RoleName && rp.Department == permission.Department);

            if (existing == null)
            {
                // Verify role exists
                if (await _roleManager.FindByNameAsync(permission.RoleName) == null)
                {
                    throw new InvalidOperationException($"Role '{permission.RoleName}' does not exist");
                }

                var per = mapper.Map<RolePermissions>(permission);
                _context.RolePermissions.Add(per);
            }
            else
            {
                existing.View = permission.View;
                existing.Add = permission.Add;
                existing.Edit = permission.Edit;
                existing.Delete = permission.Delete;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<RolePermissions> GetRolePermissionsAsync(string roleName, Department department)
        {
            return await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleName == roleName && rp.Department == department);
        }

        public async Task<List<RolePermissions>> GetAllRolePermissionsAsync(string roleName)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleName == roleName)
                .ToListAsync();
        }

        public async Task BulkUpdatePermissionsAsync(List<PermissionDTO> permissions)
        {
            foreach (var permission in permissions)
            {
                var existing = await _context.RolePermissions.FindAsync(permission.RoleName, permission.Department);
                if (existing == null)
                {
                    throw new InvalidOperationException($"Permission not found for role {permission.RoleName} and department {permission.Department}");
                }

                existing.View = permission.View;
                existing.Add = permission.Add;
                existing.Edit = permission.Edit;
                existing.Delete = permission.Delete;
            }

            await _context.SaveChangesAsync();
        }


    }
}

