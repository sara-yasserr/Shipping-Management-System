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
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<DeliveryAgent> DeliveryAgent { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        



        public ShippingDBContext(DbContextOptions<ShippingDBContext> options): base(options)
        {
        }
    }
}
