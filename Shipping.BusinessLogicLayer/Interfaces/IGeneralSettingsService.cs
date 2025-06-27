using Shipping.BusinessLogicLayer.DTOs.GeneralSettingsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IGeneralSettingsService
    {
        Task<ReadGeneralSettingsDTO> GetGeneralSettingsAsync();
        Task UpdateGeneralSettingsAsync(UpdateGeneralSettingsDTO settings);
    }
}
