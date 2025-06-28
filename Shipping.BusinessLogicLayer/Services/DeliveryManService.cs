using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Shipping.BusinessLogicLayer.Services
{
    public class DeliveryManService : IDeliveryManService
    {
        private readonly UnitOfWork _unitOfWork;
        public DeliveryManService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ReadDeliveryMan>> GetAllAsync()
        {
            var deliveryMen = _unitOfWork.DeliveryManRepo.GetAll();
            var result = deliveryMen.Select(deliveryMan => new ReadDeliveryMan
            {
                Id = deliveryMan.Id,
                FullName = deliveryMan.User != null ? deliveryMan.User.FirstName + " " + deliveryMan.User.LastName : null,
                UserName = deliveryMan.User?.UserName,
                Email = deliveryMan.User?.Email,
                PhoneNumber = deliveryMan.User?.PhoneNumber,
                BranchName = deliveryMan.Branch?.Name,
                Cities = deliveryMan.Cities != null ? string.Join(", ", deliveryMan.Cities.Select(c => c.Name)) : null
            }).ToList();
            return await Task.FromResult(result);
        }

        public async Task<ReadDeliveryMan> GetByIdAsync(int id)
        {
            var deliveryMan = _unitOfWork.DeliveryManRepo.GetById(id);
            if (deliveryMan == null) return null;
            var dto = new ReadDeliveryMan
            {
                Id = deliveryMan.Id,
                FullName = deliveryMan.User != null ? deliveryMan.User.FirstName + " " + deliveryMan.User.LastName : null,
                UserName = deliveryMan.User?.UserName,
                Email = deliveryMan.User?.Email,
                PhoneNumber = deliveryMan.User?.PhoneNumber,
                BranchName = deliveryMan.Branch?.Name,
                Cities = deliveryMan.Cities != null ? string.Join(", ", deliveryMan.Cities.Select(c => c.Name)) : null
            };
            return await Task.FromResult(dto);
        }

        public async Task AddAsync(AddDeliveryMan dto)
        {
            // Create ApplicationUser
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                FirstName = dto.Name?.Split(' ').FirstOrDefault() ?? dto.Name,
                LastName = dto.Name?.Contains(' ') == true ? dto.Name.Substring(dto.Name.IndexOf(' ') + 1) : string.Empty
            };
            var result = await _unitOfWork.UserManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new System.Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            // Create DeliveryMan
            var deliveryMan = new DeliveryAgent
            {
                BranchId = dto.BranchId,
                UserId = user.Id,
                Cities = dto.CityIds?.Select(id => _unitOfWork.db.Cities.Find(id)).ToList()
            };
            _unitOfWork.DeliveryManRepo.Add(deliveryMan);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(int id, AddDeliveryMan dto)
        {
            var deliveryMan = _unitOfWork.DeliveryManRepo.GetById(id);
            if (deliveryMan == null) return;
            // Update user info
            var user = await _unitOfWork.UserManager.FindByIdAsync(deliveryMan.UserId);
            if (user != null)
            {
                user.UserName = dto.UserName;
                user.Email = dto.Email;
                user.PhoneNumber = dto.PhoneNumber;
                user.FirstName = dto.Name?.Split(' ').FirstOrDefault() ?? dto.Name;
                user.LastName = dto.Name?.Contains(' ') == true ? dto.Name.Substring(dto.Name.IndexOf(' ') + 1) : string.Empty;
                await _unitOfWork.UserManager.UpdateAsync(user);
                // Password update is not handled here for security reasons
            }
            deliveryMan.BranchId = dto.BranchId;
            deliveryMan.Cities = dto.CityIds?.Select(cid => _unitOfWork.db.Cities.Find(cid)).ToList();
            _unitOfWork.DeliveryManRepo.Update(deliveryMan);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var deliveryMan = _unitOfWork.DeliveryManRepo.GetById(id);
            if (deliveryMan == null) return;
            _unitOfWork.DeliveryManRepo.Delete(deliveryMan);
            await _unitOfWork.SaveAsync();
        }
    }
} 