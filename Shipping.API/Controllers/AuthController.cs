using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.AuthDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.Interfaces;
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
        private readonly IPermissionCheckerService _permissionChecker;

        public AuthController(
            UnitOfWork unitOfWork,
            JwtHelper jwtHelper,
            RoleManager<IdentityRole> roleManager,
            IPermissionCheckerService permissionChecker)
        {
            this.unitOfWork = unitOfWork;
            this._jwtHelper = jwtHelper;
            this._roleManager = roleManager;
            this._permissionChecker = permissionChecker;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid Request Data" });

            var user = await unitOfWork.UserManager.FindByNameAsync(loginDTO.userName);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var isPasswordValid = await unitOfWork.UserManager.CheckPasswordAsync(user, loginDTO.password);
            if (!isPasswordValid)
                return Unauthorized(new { message = "Invalid username or password" });

            var roles = await unitOfWork.UserManager.GetRolesAsync(user);
            var permissions = await _permissionChecker.GetUserPermissions(user);

            var token = _jwtHelper.GenerateToken(user, roles, permissions);

            return Ok(new { token });
        }

        // For Test Purpose: Create Seller
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
                PhoneNumber = dto.Phone,
                CreatedAt = DateTime.Now
            };

            var result = await unitOfWork.UserManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var seller = new Seller
            {
                UserId = user.Id,
                Address = dto.Address,
                StoreName = dto.StoreName,
                CityId = (int)dto.cityId,
                CancelledOrderPercentage = 0.5m
            };

            unitOfWork.SellerRepo.Add(seller);
            unitOfWork.Save();

            const string defaultRole = "Seller";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
                await _roleManager.CreateAsync(new IdentityRole(defaultRole));

            await unitOfWork.UserManager.AddToRoleAsync(user, defaultRole);

            return Ok(new { message = "User Created Successfully", user.Id });
        }
    }
}
