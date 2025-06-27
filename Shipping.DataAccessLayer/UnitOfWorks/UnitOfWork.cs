using Microsoft.AspNetCore.Identity;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.Repositories;
using Shipping.DataAccessLayer.Repositories.Custom;

namespace Shipping.DataAccessLayer.UnitOfWorks
{
    public class UnitOfWork
    {
        public readonly ShippingDBContext db;
        public readonly UserManager<ApplicationUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;
        private GenericRepository<Branch> _branchRepo;
        private GenericRepository<Governorate> _governorateRepo;
        private RolePermissionsRepository _rolePermissionsRepo;
        //private GenericRepository<RolePermissions> _rolePermissionsRepo;
        public UnitOfWork(ShippingDBContext db, UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager) 
        {
            this.db = db;
            _userManager = userManager;
            _roleManager = roleManager;
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
        //public GenericRepository<RolePermissions> RolePermissionsRepo
        //{
        //    get
        //    {
        //        if(_rolePermissionsRepo == null)
        //        {
        //            _rolePermissionsRepo = new GenericRepository<RolePermissions>(db);
        //        }
        //        return _rolePermissionsRepo;
        //    }
        //}
        public RolePermissionsRepository RolePermissionsRepo
        {
            get
            {
                if (_rolePermissionsRepo == null)
                {
                    _rolePermissionsRepo = new RolePermissionsRepository(db);
                }
                return _rolePermissionsRepo;
            }
        }
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
