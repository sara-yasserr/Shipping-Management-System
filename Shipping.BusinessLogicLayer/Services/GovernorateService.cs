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
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.DTOs;

namespace Shipping.BusinessLogicLayer.Services
{
    public class GovernorateService : IGovernorateService
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

        public PagedResponse<ReadGovernorateDto> GetAll(PaginationDTO pagination)
        {
            var governorates = _unitOfWork.GovernorateRepo.GetAll();
            //var governorates = _unitOfWork.GovernorateRepo.GetAll().Where(g => g.IsDeleted == false);
            var count = governorates.Count();
            var pagedGovernorates = governorates
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            var data = _mapper.Map<List<ReadGovernorateDto>>(pagedGovernorates);
            var result = new PagedResponse<ReadGovernorateDto>
            {
                Items = data,
                TotalCount = count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize)
            };
            return result;
        }

        public List<ReadGovernorateDto> GetAll()
        {
            var governorates = _unitOfWork.GovernorateRepo.GetAll();
           return _mapper.Map<List<ReadGovernorateDto>>(governorates);
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

        public void ActiveGovernorate(int id)
        {
            var foundGovernorate = _unitOfWork.GovernorateRepo.GetById(id);
            if (foundGovernorate != null)
            {
                foundGovernorate.IsDeleted = false; //Active Governorate
                _unitOfWork.GovernorateRepo.Update(foundGovernorate);
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception("Governorate not found");
            }
        }
    }
}
