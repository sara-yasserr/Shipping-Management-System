using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.City;
using Shipping.BusinessLogicLayer.Services;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {

        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CityController(UnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<List<CityDTO>> GetAll()
        {
            return Ok(unitOfWork.CityRepo.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetById(int id)
        {
            var city = unitOfWork.CityRepo.GetById(id);
            if (city == null) return NotFound();
            return Ok(city);
        }

        [HttpPost]
        public IActionResult Add(CreateCityDTO dto)
        {
            var city = mapper.Map<City>(dto);
            unitOfWork.CityRepo.Add(city);
            unitOfWork.Save();

            return Ok();     
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,UpdateCityDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var city = unitOfWork.CityRepo.GetById(id);
            mapper.Map(dto, city);
            unitOfWork.CityRepo.Update(city);
            unitOfWork.Save();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var city = unitOfWork.CityRepo.GetById(id);
            if (city == null) return NotFound();
            unitOfWork.CityRepo.Delete(city);
            return NoContent(); 
        }

    }
}
