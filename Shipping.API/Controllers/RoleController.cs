using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.BusinessLogicLayer.Services;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        [HttpGet]
        public IActionResult GetAll() {
            var roles = roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("Paginated")]
        public IActionResult GetAllPaginated([FromQuery] PaginationDTO pagination)
        {
            return Ok(roleService.GetAllPaginated(pagination));
        }

        [HttpGet("{name:alpha}")]
        public async Task<IActionResult> GetById(string name)
        {
            var role= await roleService.GetRoleByNameAsync(name);
           return Ok(role);
        }
        [HttpPost]
        public async Task<IActionResult> Post(string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await roleService.CreateRoleAsync(roleName);
            return Ok(result);
        }
        [HttpPut]
        public IActionResult Put(string oldRoleName, string newRoleName)
        {
           return Ok(roleService.UpdateRole(oldRoleName,newRoleName));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string roleName) 
        {
            var result = await roleService.DeleteRoleAsync(roleName);
            if (result.Succeeded)
                return Ok(new { success = true });

            return BadRequest(new { success = false, errors = result.Errors.Select(e => e.Description) });
        }
    }
}
