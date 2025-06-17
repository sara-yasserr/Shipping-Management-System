using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class ShippingDBContext : IdentityDbContext<ApplicationUser>
    {
        public ShippingDBContext(DbContextOptions<ShippingDBContext> options): base(options)
        {
        }
    }
}
