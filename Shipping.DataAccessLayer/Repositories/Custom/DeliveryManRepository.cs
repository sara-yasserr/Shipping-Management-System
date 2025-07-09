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

        public IQueryable<DeliveryAgent> GetAllWithIncludes()
        {
            var result =  _db.DeliveryAgent
                .Include(d => d.User)
                .Include(d => d.Branch)
                .Include(d => d.Cities)
                .Include(d => d.Orders);
            return result;
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

        // دالة HardDelete جديدة لمسح المندوب واليوزر المرتبط به فعليًا
        public async Task HardDeleteDeliveryMan(int deliveryManId)
        {
            var deliveryMan = await _db.DeliveryAgent
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == deliveryManId);

            if (deliveryMan != null)
            {
                // حذف الـ user المرتبط لو موجود
                if (deliveryMan.User != null)
                {
                    _db.Users.Remove(deliveryMan.User);
                }
                _db.DeliveryAgent.Remove(deliveryMan);
                await _db.SaveChangesAsync();
            }
        }
    }
}
