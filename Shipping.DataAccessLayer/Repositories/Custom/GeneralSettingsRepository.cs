using Shipping.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccessLayer.Repositories.Custom
{
    public class GeneralSettingsRepository
    {
        private ShippingDBContext _context;
        public GeneralSettingsRepository(ShippingDBContext context) {
            _context = context;
        }
        public GeneralSetting GetGeneralSettings()
        {
            return _context.GeneralSettings.FirstOrDefault();
        }

        public void UpdateGeneralSettings(GeneralSetting settings)
        {
            var existingSettings = _context.GeneralSettings.FirstOrDefault();
            if (existingSettings != null)
            {
                existingSettings.DefaultWeight = settings.DefaultWeight;
                existingSettings.ExtraPriceKg = settings.ExtraPriceKg;
                existingSettings.ExtraPriceVillage = settings.ExtraPriceVillage;
                existingSettings.Fast = settings.Fast;
                existingSettings.Express = settings.Express;
                existingSettings.ModifiedAt = DateTime.Now;
                existingSettings.EmployeeId = settings.EmployeeId;
                //_context.SaveChanges();
            }
            else
            {
                // If no settings exist, add the new one
                _context.GeneralSettings.Add(settings);
                //_context.SaveChanges();
            }
        }
    }
}
