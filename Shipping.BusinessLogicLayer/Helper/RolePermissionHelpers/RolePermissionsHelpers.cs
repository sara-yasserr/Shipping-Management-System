using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;

namespace Shipping.BusinessLogicLayer.Helper.RolePermissionHelpers
{
    // =======================
    // DepartmentPermissionRequirement
    // =======================
    public class DepartmentPermissionRequirement : IAuthorizationRequirement
    {
        public Department? Department { get; }
        public PermissionType PermissionType { get; }

        // Constructor for general (no department specified)
        public DepartmentPermissionRequirement(PermissionType permissionType)
        {
            PermissionType = permissionType;
        }

        // Constructor for department-specific policies
        public DepartmentPermissionRequirement(Department department, PermissionType permissionType)
        {
            Department = department;
            PermissionType = permissionType;
        }
    }

    // =======================
    // DepartmentPermissionHandler
    // =======================
    public class DepartmentPermissionHandler : AuthorizationHandler<DepartmentPermissionRequirement>
    {
        private readonly IPermissionService _permissionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentPermissionHandler(
            IPermissionService permissionService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _permissionService = permissionService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DepartmentPermissionRequirement requirement)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null) return;

            var roles = await _userManager.GetRolesAsync(user);

            // Determine the department: from requirement first, or route values
            Department? department = requirement.Department;

            if (!department.HasValue)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var routeValues = httpContext?.GetRouteData()?.Values;

                if (routeValues != null && routeValues.TryGetValue("department", out var deptValue))
                {
                    if (Enum.TryParse<Department>(deptValue?.ToString(), true, out var parsedDepartment))
                    {
                        department = parsedDepartment;
                    }
                }
            }

            foreach (var roleName in roles)
            {
                if (department.HasValue)
                {
                    if (await _permissionService.HasPermissionAsync(roleName, department.Value, requirement.PermissionType))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
                else
                {
                    if (await _permissionService.HasGeneralPermissionAsync(roleName, requirement.PermissionType))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
        }
    }
}
