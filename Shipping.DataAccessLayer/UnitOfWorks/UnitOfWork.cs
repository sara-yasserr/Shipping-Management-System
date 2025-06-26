using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.Repositories;

namespace Shipping.DataAccessLayer.UnitOfWorks
{
    public class UnitOfWork
    {
        private readonly ShippingDBContext db;
        private GenericRepository<Branch> _branchRepo;
        private GenericRepository<City> _cityRepo;
        public UnitOfWork(ShippingDBContext db) 
        {
            this.db = db;
        }

        public GenericRepository<City> CityRepo
        {
            get
            {
                if (_cityRepo == null)
                {
                    _cityRepo = new GenericRepository<City>(db);
                }
                return _cityRepo;
            }
        }

        #region Props
        public GenericRepository<Branch> BranchRepo
        {
            get
            {
                if(_branchRepo == null)
                {
                    _branchRepo = new GenericRepository<Branch>(db);
                }
                return _branchRepo;
            }
        }
        #endregion
        public int Save()
        {
            return db.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await db.SaveChangesAsync();
        }
    }
}
