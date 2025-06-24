using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.DataAccessLayer.Models;

namespace Shipping.DataAccessLayer.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private readonly ShippingDBContext db;
        public GenericRepository(ShippingDBContext db) {
        this.db = db;
        }

        public List<TEntity> GetAll()
        {
           return db.Set<TEntity>().ToList();
        }
        public TEntity? GetById(int id)
        {
            return db.Set<TEntity>().Find(id);
        }
        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            db.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
        }
    }
}
