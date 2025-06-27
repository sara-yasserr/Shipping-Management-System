using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;
using Shipping.DataAccessLayer.UnitOfWorks;
using Shipping.DataAccessLayer.Models;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GovernorateController(UnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(unitOfWork.GovernorateRepo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = unitOfWork.GovernorateRepo.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(AddGovernorateDto dto)
        {
            if (!ModelState.IsValid) { 
            return BadRequest(ModelState); }
            var governorate = mapper.Map<Governorate>(dto);
            unitOfWork.GovernorateRepo.Add(governorate);
            unitOfWork.Save();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, AddGovernorateDto dto)
        {
            var governorate = unitOfWork.GovernorateRepo.GetById(id);
            mapper.Map(dto,governorate);
            unitOfWork.GovernorateRepo.Update(governorate);
            unitOfWork.Save();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var governorate = unitOfWork.GovernorateRepo.GetById(id);
            unitOfWork.GovernorateRepo.Delete(governorate);
            return Ok();
        }
    }
}
