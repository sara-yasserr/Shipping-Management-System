using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Shipping.BusinessLogicLayer.DTOs;

namespace Shipping.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryManController : ControllerBase
    {
        private readonly IDeliveryManService _service;
        public DeliveryManController(IDeliveryManService service)
        {
            _service = service;
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetAll([FromQuery]PaginationDTO pagination)
        {
            try
            {
                var result = await _service.GetAllAsync(pagination);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithoutPagination()
        {
            try
            {
                var result =  _service.GetAll();
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { error = "Invalid ID. ID must be greater than 0." });
            }

            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) return NotFound(new { error = "Delivery man not found" });
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddDeliveryMan dto)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(new { error = "Validation failed", details = ModelState });
            // }

            try
            {
                var (success, deliveryManId) = await _service.AddAsync(dto);
                if (success)
                    return Ok(new { message = "Delivery man added successfully", id = deliveryManId });
                else
                    return BadRequest(new { error = "Failed to add delivery man" });
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("already taken") || ex.Message.Contains("already registered"))
                    return Conflict(new { error = ex.Message });
                return BadRequest(new { error = ex.Message, details = ex.StackTrace });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeliveryMan dto)
        {
            if (id <= 0)
            {
                return BadRequest(new { error = "Invalid ID. ID must be greater than 0." });
            }

            // Handle password validation manually
            if (!string.IsNullOrEmpty(dto.Password))
            {
                // Only validate password if it's provided
                if (dto.Password.Length < 8)
                {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long.");
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-={}:;<>.,?]).+$"))
                {
                    ModelState.AddModelError("Password", "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Validation failed", details = ModelState });
            }

            try
            {
                var success = await _service.UpdateAsync(id, dto);
                if (success)
                    return Ok(new { message = "Delivery man updated successfully" });
                else
                    return BadRequest(new { error = "Failed to update delivery man" });
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new { error = ex.Message });
                if (ex.Message.Contains("already taken") || ex.Message.Contains("already registered"))
                    return Conflict(new { error = ex.Message });
                return BadRequest(new { error = ex.Message });
            }
        }

           [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { error = "Invalid ID. ID must be greater than 0." });
            }

            try
            {
                var success = await _service.SoftDeleteAsync(id);
                if (success)
                    return Ok(new { message = "Delivery man soft deleted successfully" });
                else
                    return BadRequest(new { error = "Failed to soft delete delivery man" });
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new { error = ex.Message });
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("HardDelete/{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { error = "Invalid ID. ID must be greater than 0." });
            }

            try
            {
                var success = await _service.HardDeleteAsync(id);
                if (success)
                    return Ok(new { message = "Delivery man hard deleted successfully" });
                else
                    return BadRequest(new { error = "Failed to hard delete delivery man" });
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(new { error = ex.Message });
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 