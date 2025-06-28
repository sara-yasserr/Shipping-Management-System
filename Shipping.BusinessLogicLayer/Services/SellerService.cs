using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.Seller;
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

        public SellerService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<SellerDTO> GetAll()
        {
            var sellers = _unitOfWork.SellerRepo.GetAll();
            return _mapper.Map<List<SellerDTO>>(sellers);
        }

        public SellerDTO? GetById(int id)
        {
            var seller = _unitOfWork.SellerRepo.GetById(id);
            return seller == null ? null : _mapper.Map<SellerDTO>(seller);
        }

        public void Add(AddSellerDTO dto)
        {
            var seller = _mapper.Map<Seller>(dto);
            _unitOfWork.SellerRepo.Add(seller);
            _unitOfWork.Save();
        }

        public bool Update(UpdateSellerDTO dto)
        {
            var seller = _unitOfWork.SellerRepo.GetById(dto.Id);
            if (seller == null) return false;

            _mapper.Map(dto, seller);
            _unitOfWork.SellerRepo.Update(seller);
            _unitOfWork.Save();
            return true;
        }


        public bool Delete(int id)
        {
            var seller = _unitOfWork.SellerRepo.GetById(id);
            if (seller == null) return false;

            _unitOfWork.SellerRepo.Delete(seller);
            _unitOfWork.Save();
            return true;
        }








    }
}
