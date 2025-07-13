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

        public async Task<Dictionary<string, List<string>>> GetUserPermissions(ApplicationUser user)
        {
            var permissionsMap = new Dictionary<string, List<string>>();

            var roles = await _unitOfWork.UserManager.GetRolesAsync(user);

            // إذا كان الموظف فقط، نعطيه صلاحيات كاملة لكل الأقسام
            if (roles.Count == 1 && roles.Contains("Employee"))
            {
                foreach (Department dept in Enum.GetValues(typeof(Department)))
                {
                    permissionsMap[dept.ToString()] = new List<string> { "Add", "Edit", "Delete", "View" };
                }
                return permissionsMap;
            }

            foreach (Department dept in Enum.GetValues(typeof(Department)))
            {
                var permissionList = new List<string>();

                foreach (var role in roles.Where(r => r != "Employee"))
                {
                    var permission = await _unitOfWork.RolePermissionsRepo.GetByRoleAndDepartment(role, dept);
                    if (permission == null)
                        continue;

                    if (permission.Add && !permissionList.Contains("Add"))
                        permissionList.Add("Add");
                    if (permission.Edit && !permissionList.Contains("Edit"))
                        permissionList.Add("Edit");
                    if (permission.Delete && !permissionList.Contains("Delete"))
                        permissionList.Add("Delete");
                    if (permission.View && !permissionList.Contains("View"))
                        permissionList.Add("View");
                }

                if (permissionList.Count > 0)
                    permissionsMap[dept.ToString()] = permissionList;
            }

            return permissionsMap;
        }

    }
}
