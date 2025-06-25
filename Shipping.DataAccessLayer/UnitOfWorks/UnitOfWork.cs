using Microsoft.AspNetCore.Identity;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.Repositories;

namespace Shipping.DataAccessLayer.UnitOfWorks
{
    public class UnitOfWork
    {
        private readonly ShippingDBContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private GenericRepository<Branch> _branchRepo;

        public UnitOfWork(ShippingDBContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }
        #region Props
        public GenericRepository<Branch> BranchRepo
        {
            get
            {
                if (_branchRepo == null)
                {
                    _branchRepo = new GenericRepository<Branch>(db);
                }
                return _branchRepo;
            }
        }
        public UserManager<ApplicationUser> UserManager => _userManager;
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
