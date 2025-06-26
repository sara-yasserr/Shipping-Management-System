using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Services
{
    public class PermissionCheckerService : IPermissionCheckerService
    {
        private UnitOfWork _unitOfWork;

        public PermissionCheckerService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //By User
        public async Task<bool> HasPermission(ApplicationUser user, Department department, Permissions permissionType)
        {
            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);

            if (roles.Count == 1 && roles.Contains("Employee"))
                return true;

            foreach (var roleName in roles.Where(r => r != "Employee"))
            {
                if (await HasPermission(roleName, department, permissionType))
                    return true;
            }

            return false;
        }
        //By Role Name
        public async Task<bool> HasPermission (string roleName , Department department , Permissions permissionType)
        {
            if (roleName == "Employee")
                return true;

            var permission = await _unitOfWork.RolePermissionsRepo.GetByRoleAndDepartment(roleName, department);
            if (permission == null)
                return false;

            return permissionType switch
            {
                Permissions.Add => permission.Add,
                Permissions.Edit => permission.Edit,
                Permissions.Delete => permission.Delete,
                Permissions.View => permission.View,
                _ => false
            };
        }
    }
}
