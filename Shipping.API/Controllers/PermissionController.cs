using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionCheckerService _permissionCheckerService;
        private readonly IPermissionService _permissionService;
        IMapper mapper;
        UnitOfWork unitOfWork;

        public PermissionController( UnitOfWork unitOfWork, IPermissionService permissionService, IMapper mapper, IPermissionCheckerService permissionCheckerService)
        {
            _permissionService = permissionService;
            this.mapper = mapper;
            this._permissionCheckerService = permissionCheckerService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{roleName}/{department}")]
        //[Authorize(Policy = "Admin.Permissions.View")]
        public async Task<ActionResult<RolePermissions>> GetPermissions(string roleName, Department department)
        {
            var permissions = await _permissionService.GetRolePermissionsAsync(roleName, department);
            if (permissions == null) return NotFound();
            return Ok(permissions);
        }
        //[Authorize(Policy = "Permissions.View.All")]
        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetAllPermissions(string roleName)
        {
            var permissions = await _permissionService.GetAllRolePermissionsAsync(roleName);
            if (permissions == null || permissions.Count == 0)
                return NotFound();

            return Ok(permissions);
        }

        [HttpGet("secure/{department}")]
        public async Task<IActionResult> SecureCheck(Department department)
        {
            var user = await unitOfWork._userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var hasPermission = await _permissionCheckerService.HasPermission(user, department, Permissions.View);

            if (!hasPermission)
                return Forbid(); // or return Unauthorized();

            // Proceed with logic
            return Ok("You have permission to view");
        }


        [HttpPut]
        //[Authorize(Policy = "Admin.Permissions.Edit")]
        public async Task<IActionResult> UpdatePermissions(PermissionDTO model)
        {
            try
            {
                await _permissionService.UpdateRolePermissionsAsync(model);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("bulk-update")]
        //[Authorize(Policy = "Admin.AllPermissions.Edit")]
        public async Task<IActionResult> BulkUpdatePermissions([FromBody] List<PermissionDTO> permissions)
        {
            try
            {
                await _permissionService.BulkUpdatePermissionsAsync(permissions);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating permissions");
            }
        }
    }
}
