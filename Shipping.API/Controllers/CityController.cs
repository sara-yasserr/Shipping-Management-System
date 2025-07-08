using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.City;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.Services;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {

        private readonly CityService _cityService;

        public CityController(CityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("paginated")]
        public ActionResult<PagedResponse<CityDTO>> GetAll([FromQuery] PaginationDTO pagination)
        {
            return Ok(_cityService.GetAll(pagination));
        }

        [HttpGet]
        public ActionResult<List<CityDTO>> GetAllWithoutPagination()
        {
            return Ok(_cityService.GetAllWithOutPagination());
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetById(int id)
        {
            try
            {
                var city = _cityService.GetById(id);
                if (city == null) return NotFound();
                return Ok(city);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the city.");
            }
        }

        [HttpPost]
        public IActionResult Add(CreateCityDTO dto)
        {
            _cityService.Add(dto);


            return Ok();     
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCityDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();

            _cityService.Update(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var city = _cityService.GetById(id);
            if (city == null) return NotFound();
            _cityService.Delete(id);
            return NoContent(); 
        }

    }
}


