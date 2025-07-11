﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using Shipping.BusinessLogicLayer.DTOs.Seller;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Services
{
    public class SellerService
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public SellerService(UnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager; 
        }

       

        public PagedResponse<SellerDTO> GetAll(PaginationDTO pagination)
        {
            var sellers = _unitOfWork.SellerRepo.GetAll()
                            .Where(s => s.User != null);

            //var sellers = _unitOfWork.SellerRepo.GetAll().Where(s => s.User != null);
            var count = sellers.Count();
            var pagedSellers = sellers
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            var data = _mapper.Map<List<SellerDTO>>(pagedSellers);
            var result = new PagedResponse<SellerDTO>
            {
                Items = data,
                TotalCount = count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize)
            };

            return result;
        }

        public List<SellerDTO> GetAllWithoutPagination()
        {
            var sellers = _unitOfWork.SellerRepo.GetAll()
                            .Where(s => s.User != null && s.User.IsDeleted != true)
                            .ToList();
            return _mapper.Map<List<SellerDTO>>(sellers);
        }



        public SellerDTO? GetById(int id)
        {
            var seller = _unitOfWork.SellerRepo.GetById(id);

            if (seller == null || seller.User == null )
                return null;

            return _mapper.Map<SellerDTO>(seller);
        }


        public async Task<IdentityResult> AddAsync(AddSellerDTO dto)
        {
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                return IdentityResult.Failed(new IdentityError { Description = "Username already exists." });

            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                return IdentityResult.Failed(new IdentityError { Description = "Email already exists." });

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return result;

            await _userManager.AddToRoleAsync(user, "Seller");

            var seller = _mapper.Map<Seller>(dto);
            seller.UserId = user.Id;

            _unitOfWork.SellerRepo.Add(seller);
            await _unitOfWork.SaveAsync();

            return IdentityResult.Success;
        }



        public async Task<bool> UpdateAsync(int id,UpdateSellerDTO dto)
        {
            var seller = _unitOfWork.SellerRepo.GetById(id);

            //if (seller == null || seller.User == null )
            //    return false;
            var passHashed = seller.User.PasswordHash;
            var user = seller.User;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.IsDeleted = dto.IsDeleted;
            await _userManager.UpdateAsync(user);
            
            _mapper.Map(dto, seller);
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (!result.Succeeded)
                    throw new Exception("Failed to update password: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            else
            {
                seller.User.PasswordHash = passHashed;
            }
            _unitOfWork.SellerRepo.Update(seller);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var seller = _unitOfWork.SellerRepo.GetById(id);
            if (seller == null || seller.User == null || seller.User.IsDeleted == true)
                return false;

            seller.User.IsDeleted = true;
            await _userManager.UpdateAsync(seller.User);
            _unitOfWork.SellerRepo.Update(seller);
            await _unitOfWork.SaveAsync();

            return true;
        }


        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var seller = _unitOfWork.SellerRepo.GetById(id);
        //    if (seller == null)
        //        return false;

        //    var user = seller.User;
        //    if (user != null)
        //    {
        //        await _userManager.DeleteAsync(user);
        //    }

        //    _unitOfWork.SellerRepo.Delete(seller); 
        //    await _unitOfWork.SaveAsync();

        //    return true;
        //}

        public SellerDTO GetByUserId(string UserId)
        {
            var seller = _unitOfWork.SellerRepo.GetAll().FirstOrDefault(s => s.UserId == UserId);
            return _mapper.Map<SellerDTO>(seller);
        }


    }
}
