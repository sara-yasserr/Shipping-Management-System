using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        
        public List<TEntity> GetAllWithInclude(params string[] includes)
        {
            var query = db.Set<TEntity>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.ToList();
        }
        
        public TEntity? GetById(int id)
        {
            return db.Set<TEntity>().Find(id);
        }
        
        public TEntity? GetByIdWithInclude(int id, params string[] includes)
        {
            var query = db.Set<TEntity>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault();
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
        public async Task AddAsync(TEntity entity)
        {
            await db.Set<TEntity>().AddAsync(entity);
        }
    }
}
