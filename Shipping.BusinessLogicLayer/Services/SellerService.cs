using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shipping.BusinessLogicLayer.DTOs;
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
                            .Where(s => s.User != null && s.User.IsDeleted != true);
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

            if (seller == null || seller.User == null || seller.User.IsDeleted == true)
                return null;

            return _mapper.Map<SellerDTO>(seller);
        }


        public async Task<bool> AddAsync(AddSellerDTO dto)
        {
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
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                return false;
            }

            await _userManager.AddToRoleAsync(user, "Seller");

            var seller = _mapper.Map<Seller>(dto);
            seller.UserId = user.Id;

            _unitOfWork.SellerRepo.Add(seller);
            await _unitOfWork.SaveAsync();

            return true;
        }


        public async Task<bool> UpdateAsync(UpdateSellerDTO dto)
        {
            var seller = _unitOfWork.SellerRepo.GetById(dto.Id);

            if (seller == null || seller.User == null || seller.User.IsDeleted == true)
                return false;

            var user = seller.User;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            
            await _userManager.UpdateAsync(user);

            _mapper.Map(dto, seller);
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

        

     



    }
}
