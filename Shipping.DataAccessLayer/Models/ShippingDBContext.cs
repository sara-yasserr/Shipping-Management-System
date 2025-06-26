using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Shipping.DataAccessLayer.Models
{
    public class ShippingDBContext : IdentityDbContext<ApplicationUser>
    {
        public ShippingDBContext(DbContextOptions<ShippingDBContext> options) : base(options)
        {
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
