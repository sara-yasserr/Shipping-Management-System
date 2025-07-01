using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Models
{
    public class ShippingDBContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<DeliveryAgent> DeliveryAgent { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<Governorate> Governorates { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }
        



        public ShippingDBContext(DbContextOptions<ShippingDBContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add Employee, Seller and DeliveryAgent Role
            base.OnModelCreating(modelBuilder);

            string EmployeeRoleId = "Employee-ROLE-001";
            string AdminRoleId = "Admin-ROLE-001";
            string SellerRoleId = "Seller-ROLE-001";
            string DeliveryAgentRoleId = "DeliveryAgent-ROLE-001";

            string EmployeeUserId = "Employee-USER-001";

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = EmployeeRoleId,
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            },
            new IdentityRole
            {
                Id = SellerRoleId,
                Name = "Seller",
                NormalizedName = "SELLER"
            },
            new IdentityRole
            {
                Id = DeliveryAgentRoleId,
                Name = "DeliveryAgent",
                NormalizedName = "DELIVERYAGENT"
            },
            new IdentityRole
            {
                Id = AdminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = EmployeeUserId,
                UserName = "employee",
                NormalizedUserName = "EMPLOYEE",
                Email = "employee@shipping.com",
                NormalizedEmail = "EMPLOYEE@SHIPPING.COM",
                EmailConfirmed = true,
                //Admin@123
                PasswordHash = "AQAAAAIAAYagAAAAEIjJh6/LXD2Bg+3MJGc+CmiaE471FJWBEmlTQ/1OhqkFw0NIgG/beU7wkTfmnuQ/sQ==",
                SecurityStamp = "STATIC-SECURITY-STAMP-001",
                ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-001",
                FirstName = "Admin",
                LastName = "User",
                PhoneNumber = "01026299485",
                CreatedAt = new DateTime(2025, 6, 25, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = EmployeeUserId,
                RoleId = EmployeeRoleId
            },
            new IdentityUserRole<string>
            {
                UserId = EmployeeUserId,
                RoleId = AdminRoleId
            });

            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                Id = 1,
                UserId = EmployeeUserId,
                SpecificRole = "Admin"
            });


            modelBuilder.Entity<City>()
            .HasMany(c => c.DeliveryAgents)
            .WithMany(d => d.Cities)
            .UsingEntity<Dictionary<string, object>>(
                "CityDeliveryAgent",
                j => j
                    .HasOne<DeliveryAgent>()
                    .WithMany()
                    .HasForeignKey("DeliveryAgentsId")
                    .OnDelete(DeleteBehavior.NoAction),
                j => j
                    .HasOne<City>()
                    .WithMany()
                    .HasForeignKey("CitiesId")
                    .OnDelete(DeleteBehavior.NoAction)
            );
            //Seed General Settings

            modelBuilder.Entity<GeneralSetting>().HasData(
                new GeneralSetting { Id = 1, DefaultWeight = 10, ExtraPriceKg = 5, ExtraPriceVillage = 20,
                    ModifiedAt = new DateTime(2025, 6, 25, 0, 0, 0, DateTimeKind.Utc), Fast = .2m, Express = .5m , EmployeeId = 1 }
                );

            
            foreach (var dept in System.Enum.GetValues(typeof(Shipping.DataAccessLayer.Enum.Department)).Cast<Shipping.DataAccessLayer.Enum.Department>())
            {
                modelBuilder.Entity<RolePermissions>().HasData(new RolePermissions
                {
                    RoleName = "Employee",
                    Department = dept,
                    View = true,
                    Add = true,
                    Edit = true,
                    Delete = true
                });
            }

            //composite key Role Permissions
            modelBuilder.Entity<RolePermissions>()
                .HasKey(rp => new { rp.RoleName, rp.Department });

           
        }
    }
}
