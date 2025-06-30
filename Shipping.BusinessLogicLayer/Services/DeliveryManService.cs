using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System;

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

              
                var user = deliveryMan.User;
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                    throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));

                
                deliveryMan.UserId = user.Id;

                
                await _unitOfWork.DeliveryManRepo.AddAsync(deliveryMan);
                await _unitOfWork.SaveAsync();

             
                if (dto.CityIds?.Any() == true)
                {
                    await _unitOfWork.DeliveryManRepo.UpdateDeliveryManCities(deliveryMan.Id, dto.CityIds);
                }

                
                await _userManager.AddToRoleAsync(user, "DeliveryAgent");

                return (true, deliveryMan.Id);
            }
            catch (Exception ex)
            {
                throw new Exception("AddDeliveryMan Error: " + ex.Message, ex);
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
                
                // Update password only if a new password is provided
                if (!string.IsNullOrEmpty(dto.Password))
                {
                    var user = deliveryMan.User;
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                    if (!result.Succeeded)
                        throw new Exception("Failed to update password: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                
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

                // Set DeliveryAgentId = null for all related orders
                var orders = _unitOfWork.db.Orders.Where(o => o.DeliveryAgentId == id).ToList();
                foreach (var order in orders)
                {
                    order.DeliveryAgentId = null;
                }
                await _unitOfWork.SaveAsync();

                // Delete the delivery man
                _unitOfWork.DeliveryManRepo.Delete(deliveryMan);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var deliveryMan = _unitOfWork.DeliveryManRepo.GetByIdWithIncludes(id);
                if (deliveryMan == null) return false;
                
                // Check if delivery man has active orders
                var activeOrders = deliveryMan.Orders?.Where(o => o.IsActive).ToList();
                if (activeOrders?.Any() == true)
                {
                    throw new System.Exception($"Cannot delete delivery man. There are {activeOrders.Count} active orders assigned to this delivery man. Please reassign or complete these orders first.");
                }
                
                // Check if delivery man has any orders (even deleted ones)
                var totalOrders = deliveryMan.Orders?.Count ?? 0;
                if (totalOrders > 0)
                {
                    // Just clear cities and return (no hard delete)
                    deliveryMan.Cities.Clear();
                    _unitOfWork.DeliveryManRepo.Update(deliveryMan);
                    await _unitOfWork.SaveAsync();
                    throw new System.Exception($"Delivery man has {totalOrders} orders in history. The delivery man was not deleted but cities were cleared.");
                }
                
                // No orders found - safe to hard delete
                _unitOfWork.DeliveryManRepo.Delete(deliveryMan);
                await _unitOfWork.SaveAsync();
                
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
} 