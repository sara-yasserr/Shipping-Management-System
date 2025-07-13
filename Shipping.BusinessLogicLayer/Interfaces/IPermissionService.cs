using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(string roleName, Department department, PermissionType permissionType);
        Task<RolePermissions> GetRolePermissionsAsync(string roleName, Department department);
        Task UpdateRolePermissionsAsync(PermissionDTO permission);
        Task<List<RolePermissions>> GetAllRolePermissionsAsync(string roleName);
        Task<bool> HasGeneralPermissionAsync(string roleName, PermissionType permissionType);
        Task BulkUpdatePermissionsAsync(List<PermissionDTO> permissions);


    }

    public enum PermissionType
    {
        View,
        Add,
        Edit,
        Delete
    }
}
