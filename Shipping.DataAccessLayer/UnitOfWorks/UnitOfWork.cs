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
        private GenericRepository<City> _cityRepo;
        private GenericRepository<Seller> _sellerRepo;

        //public UnitOfWork(ShippingDBContext db) 
        private GenericRepository<Governorate> _governorateRepo;
        private RolePermissionsRepository _rolePermissionsRepo;
        //private GenericRepository<RolePermissions> _rolePermissionsRepo;
        private GenericRepository<Employee> _employeeRepo;
        private GeneralSettingsRepository _generalSettingsRepo;
        public UnitOfWork(ShippingDBContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region City
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

        #endregion

        #region Branch

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


        public GenericRepository<Seller> SellerRepo
        {
            get
            {
                if (_sellerRepo == null)
                {
                    _sellerRepo = new GenericRepository<Seller>(db);
                }
                return _sellerRepo;
            }
        }








        public UserManager<ApplicationUser> UserManager => _userManager;
        #endregion

        #region Governorate
        public GenericRepository<Governorate> GovernorateRepo
        {
            get
            {
                if (_governorateRepo == null)
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

        
        #region RolePermissions
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
        #endregion

        #region Employee
        public GenericRepository<Employee> EmployeeRepo
        {
            get
            {
                if (_employeeRepo == null)
                {
                    _employeeRepo = new GenericRepository<Employee>(db);
                }
                return _employeeRepo;
            }
        }
        #endregion

        #region GeneralSettings
        public GeneralSettingsRepository GeneralSettingsRepo
        {
            get
            {
                if (_generalSettingsRepo == null)
                {
                    _generalSettingsRepo = new GeneralSettingsRepository(db);
                }
                return _generalSettingsRepo;
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
        #endregion
    }
}