using Shipping.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Repositories.Custom
{
    public class DeliveryManRepository : GenericRepository<DeliveryAgent>
    {
        private readonly ShippingDBContext _db;
        public DeliveryManRepository(ShippingDBContext db) : base(db)
        {
            _db = db;
        }

        public List<DeliveryAgent> GetAllWithIncludes()
        {
            return _db.DeliveryAgent
                .Include(d => d.User)
                .Include(d => d.Branch)
                .Include(d => d.Cities)
                .Include(d => d.Orders)
                .ToList();
        }

        public DeliveryAgent GetByIdWithIncludes(int id)
        {
            return _db.DeliveryAgent
                .Include(d => d.User)
                .Include(d => d.Branch)
                .Include(d => d.Cities)
                .Include(d => d.Orders)
                .FirstOrDefault(d => d.Id == id);
        }

        public async Task SoftDeleteDeliveryMan(int deliveryManId)
        {
            var deliveryMan = await _db.DeliveryAgent
                .Include(d => d.User)
                .Include(d => d.Cities)
                .FirstOrDefaultAsync(d => d.Id == deliveryManId);

            if (deliveryMan != null)
            {
                deliveryMan.User.IsDeleted = true;
                deliveryMan.Cities.Clear();
                _db.DeliveryAgent.Update(deliveryMan);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateDeliveryManCities(int deliveryManId, List<int> cityIds)
        {
            var deliveryMan = await _db.DeliveryAgent
                .Include(d => d.Cities)
                .FirstOrDefaultAsync(d => d.Id == deliveryManId);

            if (deliveryMan != null)
            {
                deliveryMan.Cities.Clear();
                if (cityIds?.Any() == true)
                {
                    var cities = await _db.Cities
                        .Where(c => cityIds.Contains(c.Id))
                        .ToListAsync();
                    deliveryMan.Cities.AddRange(cities);
                }
                _db.DeliveryAgent.Update(deliveryMan);
                await _db.SaveChangesAsync();
            }
        }
    }
}
