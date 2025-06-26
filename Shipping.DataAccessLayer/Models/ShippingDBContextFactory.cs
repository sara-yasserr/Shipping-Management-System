using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Shipping.DataAccessLayer.Models
{
    internal class ShippingDBContextFactory : IDesignTimeDbContextFactory<ShippingDBContext>
    {
        public ShippingDBContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Shipping.API"))
                .AddJsonFile("appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ShippingDBContext>();
            optionsBuilder.UseLazyLoadingProxies()
                          .UseSqlServer(configuration.GetConnectionString("ShippingCS"));

            return new ShippingDBContext(optionsBuilder.Options);
        }
    }
}
