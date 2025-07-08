using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.Services;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.UnitOfWorks;

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
    }
} 