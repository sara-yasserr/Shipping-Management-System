using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            string SellerUserId = "Seller-USER-001";

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

            modelBuilder.Entity<ApplicationUser>().HasData( new ApplicationUser{
                Id = SellerUserId,
                UserName = "seller",
                NormalizedUserName = "SELLER",
                Email = "seller@shipping.com",
                NormalizedEmail = "SELLER@SHIPPING.COM",
                EmailConfirmed = true,
                //Admin@123
                PasswordHash = "AQAAAAIAAYagAAAAEIjJh6/LXD2Bg+3MJGc+CmiaE471FJWBEmlTQ/1OhqkFw0NIgG/beU7wkTfmnuQ/sQ==",
                SecurityStamp = "STATIC-SECURITY-STAMP-001",
                ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-001",
                FirstName = "Seller",
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

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = SellerUserId,
                RoleId = SellerRoleId
            });

            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                Id = 1,
                UserId = EmployeeUserId,
                SpecificRole = "Admin"
            });
            modelBuilder.Entity<Governorate>().HasData(new Governorate
            {
                Id = 1,
                Name = "Qaloubia",
            });
            modelBuilder.Entity<City>().HasData(new City
            {
                Id = 1,
                Name = "Qalyub",
                NormalPrice = 50.00m,
                PickupPrice = 30.00m,
                GovernorateId = 1 // Assuming a governorate with Id 1 exists
            });
            modelBuilder.Entity<Branch>().HasData(new Branch
            {
                Id = 1,
                Name = "Main Branch",
                CityId = 1 // Assuming a city with Id 1 exists
            });
            modelBuilder.Entity<Seller>().HasData(new Seller
            {
                Id = 1,
                StoreName = "Main Store",
                Address = "123 Main St, City Center",
                CancelledOrderPercentage = 0.05m,
                CityId = 1, // Assuming a city with Id 1 exists
                UserId = SellerUserId
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

        public DbSet<Branch> branches { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<DeliveryAgent> deliveryAgents { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<GeneralSetting> generalSettings { get; set; }
        public DbSet<Governorate> governorates { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Seller> sellers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<City>()
                .HasMany(c => c.DeliveryAgents)
                .WithMany(d => d.Cities)
                .UsingEntity<Dictionary<string, object>>(
                    "CityDeliveryAgent",
                    j => j
                        .HasOne<DeliveryAgent>()
                        .WithMany()
                        .HasForeignKey("DeliveryAgentsId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<City>()
                        .WithMany()
                        .HasForeignKey("CitiesId")
                        .OnDelete(DeleteBehavior.Restrict)
                );


            modelBuilder.Entity<Order>()
                .HasOne(o => o.City)
                .WithMany()
                .HasForeignKey(o => o.CityId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeliveryAgent)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DeliveryAgentId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.Branch)
                .WithMany()
                .HasForeignKey(o => o.BranchId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.Seller)
                .WithMany()
                .HasForeignKey(o => o.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
