using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IPermissionCheckerService
    {
        Task<bool> HasPermission(ApplicationUser user, Department department, Permissions permissionType);
        Task<bool> HasPermission(string roleName, Department department, Permissions permissionType);
    }
}
