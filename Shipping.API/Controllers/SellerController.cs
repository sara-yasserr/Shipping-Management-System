using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.City;
using Shipping.BusinessLogicLayer.DTOs.Seller;
using Shipping.BusinessLogicLayer.Helper;
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
        public ActionResult<PagedResponse<SellerDTO>> GetAll([FromQuery] PaginationDTO pagination)
        {
            return Ok(_service.GetAll(pagination));
        }

        [HttpGet("{id}")]
        public ActionResult<SellerDTO> GetById(int id)
        {
            var seller = _service.GetById(id);
            if (seller == null) return NotFound();
            return Ok(seller);
        }

       

        [HttpPost]
        public async Task<IActionResult> Add(AddSellerDTO dto)
        {
            var result = await _service.AddAsync(dto);

            if (!result)
                return BadRequest("Failed to create seller.");

            return Ok("Seller created successfully.");
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateSellerDTO dto)
        {
            var result = await _service.UpdateAsync(dto);
            if (!result) return NotFound("Seller not found.");
            return Ok("✅ Seller updated successfully.");
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _service.SoftDeleteAsync(id);
            if (!result) return NotFound("Seller not found or already deleted.");
            return Ok("🗑️ Seller soft-deleted successfully.");
        }


        


    }
}
