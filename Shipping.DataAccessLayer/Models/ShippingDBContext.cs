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


        public DbSet<Branch> Branches { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<DeliveryAgent> DeliveryAgents { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seller> Sellers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.GeneralSetting)
                .WithOne(g => g.Employee)
                .HasForeignKey<GeneralSetting>(g => g.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);


            // حل سريع لمنع Multiple Cascade Paths
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // تعريف العلاقة Many-to-Many بين City و DeliveryAgent
            modelBuilder.Entity<DeliveryAgent>()
                .HasMany(d => d.Cities)
                .WithMany(c => c.DeliveryAgents)
                .UsingEntity(j => j.ToTable("CityDeliveryAgent")); // اسم الجدول الوسيط
        
        
        }

    }
}
