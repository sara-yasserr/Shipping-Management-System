using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.AuthDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;
namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly JwtHelper _jwtHelper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UnitOfWork unitOfWork, JwtHelper jwtHelper, RoleManager<IdentityRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this._jwtHelper = jwtHelper;
            this._roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invaild Request Data" });
            }

            var userFromDB = await unitOfWork.UserManager.FindByNameAsync(loginDTO.userName);
            if (userFromDB == null)
            {
                return NotFound("User not found");
            }
            var isPasswordvaild = await unitOfWork.UserManager.CheckPasswordAsync(userFromDB, loginDTO.password);
            if (!isPasswordvaild)
            {
                return Unauthorized(new { message = "Invalid UserName or Password" });
            }

            var roles = await unitOfWork.UserManager.GetRolesAsync(userFromDB);

            var token = _jwtHelper.GenerateToken(userFromDB, roles);

            return Ok(new { token });
        }

        // For Test
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.Now
            };

            var result = await unitOfWork.UserManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }


            var defaultRole = "Seller";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(defaultRole));
            }
            await unitOfWork.UserManager.AddToRoleAsync(user, defaultRole);

            return Ok(new { message = "User Created Successfully", user.Id });
        }
    }


}
