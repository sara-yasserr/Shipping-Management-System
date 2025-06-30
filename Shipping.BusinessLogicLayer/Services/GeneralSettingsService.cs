using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.GeneralSettingsDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Services
{
    public class GeneralSettingsService : IGeneralSettingsService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public GeneralSettingsService(UnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ReadGeneralSettingsDTO> GetGeneralSettingsAsync()
        {
            var settings =  _unitOfWork.GeneralSettingsRepo.GetGeneralSettings();
            if (settings == null)
            {
                throw new Exception("General settings not found.");
            }
            
            return _mapper.Map<ReadGeneralSettingsDTO>(settings);;
        }

        public async Task UpdateGeneralSettingsAsync(UpdateGeneralSettingsDTO settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings), "Settings cannot be null.");
            }
            var existingSettings = _unitOfWork.GeneralSettingsRepo.GetGeneralSettings();
            _mapper.Map(settings, existingSettings);
            _unitOfWork.GeneralSettingsRepo.UpdateGeneralSettings(existingSettings);
            await _unitOfWork.SaveAsync();
        }
    }
}
