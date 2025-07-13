using Microsoft.EntityFrameworkCore;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Repositories.Custom
{
    public class RolePermissionsRepository : GenericRepository<RolePermissions>
    {
        private ShippingDBContext _context;
        public RolePermissionsRepository(ShippingDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RolePermissions?> GetByRoleAndDepartment(string roleName, Department department)
        {
            return await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleName == roleName && rp.Department == department);
            
        }
        public async Task<List<RolePermissions>> GetByRoleNameAsync(string roleName)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleName == roleName)
                .ToListAsync();
        }

    }
}
