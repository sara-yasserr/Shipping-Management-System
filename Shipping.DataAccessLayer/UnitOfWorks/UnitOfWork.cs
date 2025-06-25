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
        private GenericRepository<Governorate> _governorateRepo;
        public UnitOfWork(ShippingDBContext db) 
        {
            this.db = db;
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

        #region Governorate
        public GenericRepository<Governorate> GovernorateRepo
        {
            get
            {
                if(_governorateRepo == null)
                {
                    _governorateRepo = new GenericRepository<Governorate>(db);
                }
                return _governorateRepo;
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
