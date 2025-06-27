using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;
using Shipping.DataAccessLayer.UnitOfWorks;
using AutoMapper;
using Shipping.DataAccessLayer.Models;

namespace Shipping.BusinessLogicLayer.Services
{
    internal class GovernorateService : IGovernorateService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GovernorateService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool AddGovernorate(AddGovernorateDto dto)
        {
            try
            {
                var governorate = _mapper.Map<Governorate>(dto);
                _unitOfWork.GovernorateRepo.Add(governorate);
                _unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public List<ReadGovernorateDto> GetAll()
        {
            var governorates = _unitOfWork.GovernorateRepo.GetAll();
            return _mapper.Map<List<Governorate>, List<ReadGovernorateDto>>(governorates.ToList());
        }

        public ReadGovernorateDto GetById(int id)
        {
            var governorate = _unitOfWork.GovernorateRepo.GetById(id);
            return _mapper.Map<Governorate, ReadGovernorateDto>(governorate);
        }

        public bool EditGovernorate(int id, AddGovernorateDto dto)
        {
            try
            {
                var foundGovernorate = _unitOfWork.GovernorateRepo.GetById(id);
                if (foundGovernorate == null)
                    return false;

                _mapper.Map(dto, foundGovernorate);
                _unitOfWork.GovernorateRepo.Update(foundGovernorate);
                _unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool SoftDeleteGovernorate(int id)
        {
            try
            {
                var foundGovernorate = _unitOfWork.GovernorateRepo.GetById(id);
                if (foundGovernorate == null)
                    return false;

                foundGovernorate.IsDeleted = true;  //Sost delete still in DataBase
                _unitOfWork.GovernorateRepo.Update(foundGovernorate);
                _unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool HardDeleteGovernorate(int id)
        {
            try
            {
                var foundGovernorate = _unitOfWork.GovernorateRepo.GetById(id);
                if (foundGovernorate == null)
                    return false;

                _unitOfWork.GovernorateRepo.Delete(foundGovernorate); //Hard Delete remove it feom DataBase
                _unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
