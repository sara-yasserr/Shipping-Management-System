using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.City;
using Shipping.BusinessLogicLayer.DTOs.Seller;
using Shipping.BusinessLogicLayer.Services;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {

        private readonly SellerService _service;

        public SellerController(SellerService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<SellerDTO>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<SellerDTO> GetById(int id)
        {
            var seller = _service.GetById(id);
            if (seller == null) return NotFound();
            return Ok(seller);
        }

        [HttpPost]
        public IActionResult Add(AddSellerDTO dto)
        {
            _service.Add(dto);
            return Ok();
        }

       
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateSellerDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();

            _service.Update(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);
            if (!result) return NotFound();
            return NoContent();

            
        }

    }
}
