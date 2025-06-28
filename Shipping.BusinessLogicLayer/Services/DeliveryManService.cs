using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Shipping.BusinessLogicLayer.Services
{
    public class DeliveryManService : IDeliveryManService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        
        public DeliveryManService(UnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            IRoleService roleService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<List<ReadDeliveryMan>> GetAllAsync()
        {
            var deliveryMen = _unitOfWork.DeliveryManRepo.GetAllWithIncludes();
            return _mapper.Map<List<ReadDeliveryMan>>(deliveryMen);
        }

        public async Task<ReadDeliveryMan> GetByIdAsync(int id)
        {
            var deliveryMan = _unitOfWork.DeliveryManRepo.GetByIdWithIncludes(id);
            if (deliveryMan == null) return null;
            
            return _mapper.Map<ReadDeliveryMan>(deliveryMan);
        }

        public async Task<(bool success, int deliveryManId)> AddAsync(AddDeliveryMan dto)
        {
            try
            {
                var deliveryMan = _mapper.Map<DeliveryAgent>(dto);

                await _unitOfWork.DeliveryManRepo.AddAsync(deliveryMan);
                await _unitOfWork.SaveAsync();

                // Add cities
                if (dto.CityIds?.Any() == true)
                {
                    await _unitOfWork.DeliveryManRepo.UpdateDeliveryManCities(deliveryMan.Id, dto.CityIds);
                }

                // Add user role "DeliveryMan" by Default
                await _userManager.AddToRoleAsync(deliveryMan.User, "DeliveryMan");

                return (true, deliveryMan.Id);
            }
            catch
            {
                return (false, 0);
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateDeliveryMan dto)
        {
            try
            {
                var deliveryMan = _unitOfWork.DeliveryManRepo.GetByIdWithIncludes(id);
                if (deliveryMan == null) return false;
                
                // Update delivery man using mapping
                _mapper.Map(dto, deliveryMan);
                
                // Update cities
                await _unitOfWork.DeliveryManRepo.UpdateDeliveryManCities(id, dto.CityIds);
                
                _unitOfWork.DeliveryManRepo.Update(deliveryMan);
                await _unitOfWork.SaveAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                var deliveryMan = _unitOfWork.DeliveryManRepo.GetByIdWithIncludes(id);
                if (deliveryMan == null) return false;

                await _unitOfWork.DeliveryManRepo.SoftDeleteDeliveryMan(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HardDeleteAsync(int id)
        {
            try
            {
                var deliveryMan = _unitOfWork.DeliveryManRepo.GetByIdWithIncludes(id);
                if (deliveryMan == null) return false;

                _unitOfWork.DeliveryManRepo.Delete(deliveryMan);
                await _unitOfWork.SaveAsync();
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 