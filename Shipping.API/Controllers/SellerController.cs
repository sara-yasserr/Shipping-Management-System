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

        [HttpGet("paginated")]
        public ActionResult<PagedResponse<SellerDTO>> GetAll([FromQuery] PaginationDTO pagination)
        {
            return Ok(_service.GetAll(pagination));
        }

        [HttpGet("{id:int}")]
        public ActionResult<SellerDTO> GetById(int id)
        {
            var seller = _service.GetById(id);
            if (seller == null) return NotFound();
            return Ok(seller);
        }

        [HttpGet("{UserId}")]
        public IActionResult GetByUserId(string UserId)
        {
            var seller = _service.GetByUserId(UserId);

            if (seller == null)
                return NotFound();

            return Ok(seller);
        }

        [HttpGet]
        public ActionResult<List<SellerDTO>> GetAllWithoutPagination()
        {
            return Ok(_service.GetAllWithoutPagination());
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddSellerDTO dto)
        {
            var result = await _service.AddAsync(dto);

            if (!result.Succeeded)
                return BadRequest(new { message = "Failed to create seller." }); // <-- مهم

            return Ok(new { message = "Seller created successfully." }); // <-- مهم
        }




        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id,UpdateSellerDTO dto)
        {
            var result = await _service.UpdateAsync(id,dto);
            if (!result) return NotFound("Seller not found.");
            return Ok();
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _service.SoftDeleteAsync(id);
            if (!result) return NotFound("Seller not found or already deleted.");
            return Ok();
        }



        [HttpGet("getId/{UserId}")]
        public IActionResult GetSellerId(string UserId)
        {
            var seller = _service.GetByUserId(UserId);

            if (seller == null)
                return NotFound();

            return Ok(seller.Id);
        }

    }
}
