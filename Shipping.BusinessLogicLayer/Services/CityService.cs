using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.City;
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
    public class CityService
    {
        private readonly UnitOfWork _unitofwork;
        private object _unitOfWork;

        public CityService(UnitOfWork unitOfWork)
        {

            _unitofwork = unitOfWork;   
        }


        public PagedResponse<CityDTO> GetAll(PaginationDTO pagination)
        {

            var cities =  _unitofwork.CityRepo.GetAll().Where(c => c.IsDeleted == false);
            var count = cities.Count();
            var pagedCities = cities
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();


            var data =  pagedCities.Select(c => new CityDTO
            {
                Id = c.Id,  
                Name = c.Name,
                NormalPrice=c.NormalPrice,
                PickupPrice=c.PickupPrice,
                GovernorateName=c.Governorate.Name

            }).ToList();

            var result = new PagedResponse<CityDTO>
            {
                TotalCount = count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize),
                Items = data
            };
            return result;

        }


        public CityDTO? GetById(int id)
        {
            var city = _unitofwork.CityRepo.GetById(id);    
            if (city == null) return null;

            return new CityDTO
            {
                Id = city.Id,
                Name = city.Name,
                NormalPrice = city.NormalPrice,
                PickupPrice = city.PickupPrice,
                GovernorateName = city.Governorate.Name
            };
        }

        public void Add(CreateCityDTO dto)
        {
            var city = new City
            {
                Name = dto.Name,
                NormalPrice = dto.NormalPrice,
                PickupPrice = dto.PickupPrice,
                GovernorateId = dto.GovernorateId
            };
            _unitofwork.CityRepo.Add(city);
            _unitofwork.Save();
        }

        public void Update(UpdateCityDTO dto)
        {
            var city = _unitofwork.CityRepo.GetById(dto.Id);
            if (city == null) return;

            city.Name = dto.Name;
            city.NormalPrice = dto.NormalPrice;
            city.PickupPrice = dto.PickupPrice;
            city.GovernorateId = dto.GovernorateId;

            _unitofwork.CityRepo.Update(city);
            _unitofwork.Save();
        }

        public void Delete(int id)
        {
            var city = _unitofwork.CityRepo.GetById(id);
            if (city == null) return;

            _unitofwork.CityRepo.Delete(city);
            _unitofwork.Save();
        }

    }
}
