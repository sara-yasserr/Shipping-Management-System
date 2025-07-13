using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Enum;
using Shipping.DataAccessLayer.Models;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        IMapper mapper;

        public PermissionController(IPermissionService permissionService, IMapper mapper)
        {
            _permissionService = permissionService;
            this.mapper = mapper;
        }

        [HttpGet("{roleName}/{department}")]
        [Authorize(Policy = "Admin.Permissions.View")]
        public async Task<ActionResult<RolePermissions>> GetPermissions(string roleName, Department department)
        {
            var permissions = await _permissionService.GetRolePermissionsAsync(roleName, department);
            if (permissions == null) return NotFound();
            return Ok(permissions);
        }
        [Authorize(Policy = "Permissions.View.All")]
        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetAllPermissions(string roleName)
        {
            var permissions = await _permissionService.GetAllRolePermissionsAsync(roleName);
            if (permissions == null || permissions.Count == 0)
                return NotFound();

            return Ok(permissions);
        }


        [HttpPut]
        [Authorize(Policy = "Admin.Permissions.Edit")]
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
        [Authorize(Policy = "Admin.AllPermissions.Edit")]
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
