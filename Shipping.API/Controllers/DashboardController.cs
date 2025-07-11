using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.Services;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;

namespace Shipping.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
  
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(UnitOfWork unitOfWork)
        {
            _dashboardService = new DashboardService(unitOfWork);
        }
        [Authorize(Roles = "Employee,Admin")]
        //api/Dashboard/overview
        [HttpGet("overview")]
        public ActionResult<DashboardDTO> GetDashboardOverview()
        {
            try
            {
                var data = _dashboardService.GetDashboardData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
          [Authorize(Roles = "DeliveryAgent")]
        // api/Dashboard/orderStatusCountsForDeliveryAgent
        [HttpGet("orderStatusCountsForDeliveryAgent")]
        public ActionResult<OrderStatusCountsDTO> GetOrderStatusCountsForDeliveryAgent()
        {
            try
            {
                
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId" || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();
                string userId = userIdClaim.Value;

                var deliveryAgent = _dashboardService.GetDeliveryAgentByUserId(userId);
                if (deliveryAgent == null)
                    return NotFound("Delivery agent not found.");
                int deliveryAgentId = deliveryAgent.Id;

                var data = _dashboardService.GetOrderStatusCountsForDeliveryAgent(deliveryAgentId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        // api/Dashboard/orderStatusCountsForSeller
        [HttpGet("orderStatusCountsForSeller")]
        [Authorize(Roles = "Seller")]
        public ActionResult<OrderStatusCountsDTO> GetOrderStatusCountsForSeller()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId" || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();
                string userId = userIdClaim.Value;

                var seller = _dashboardService.GetSellerByUserId(userId);
                if (seller == null)
                    return NotFound("Seller not found.");
                int sellerId = seller.Id;

                var data = _dashboardService.GetOrderStatusCountsForSeller(sellerId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
} 