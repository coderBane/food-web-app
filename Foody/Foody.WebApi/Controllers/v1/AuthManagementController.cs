using AutoMapper;
using Foody.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Foody.WebApi.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AuthManagementController : BaseController
    {
        readonly UserManager<IdentityUser> _userManager;

        readonly RoleManager<IdentityRole> _roleManager;

        public AuthManagementController(IUnitofWork unitofWork, IMapper mapper,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(unitofWork, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("{role}")]
        public async Task<IActionResult> AddRole(string role)
        {
            var response = await _roleManager.CreateAsync(new IdentityRole(role));

            if (!response.Succeeded)
                return BadRequest(new 
                {
                    Success = false,
                    Errors = response.Errors.Select(e => e.Description)
                });

            return Ok($"Role '{role}' added succesfully!");
        }

        [HttpPost]
        [Route("AssignRole")]
        public async Task<IActionResult> AssignRole([FromQuery] UserRole userRole)
        {
            var user = await _userManager.FindByEmailAsync(userRole.Email);

            if (user is null)
                return BadRequest(new
                {
                    Success = false,
                    Errors = "User does not exist!"
                });

            var role = await _roleManager.RoleExistsAsync(userRole.Role);

            if (!role)
                return BadRequest(new
                {
                    Success = false,
                    Errors = $"The role {userRole.Role} does not exist."
                });

            var response = await _userManager.AddToRoleAsync(user, userRole.Role);

            if (!response.Succeeded)
                return BadRequest(new
                {
                    Success = false,
                    Errors = response.Errors.Select(e => e.Description)
                });

            return Ok(new
            {
                Success = true,
                Message = $"User '{user.UserName}' now has '{userRole.Role}' priviledges."
            });
        }

        [HttpPost]
        [Route("UnAssignRole")]
        public async Task<IActionResult> UnAssignRole([FromQuery] UserRole userRole)
        {
            var user = await _userManager.FindByEmailAsync(userRole.Email);

            if (user is null)
                return BadRequest(new
                {
                    Success = false,
                    Errors = "User does not exist!"
                });

            var role = await _roleManager.RoleExistsAsync(userRole.Role);

            if (!role)
                return BadRequest(new
                {
                    Success = false,
                    Errors = $"The role {userRole.Role} does not exist."
                });

            var response = await _userManager.RemoveFromRoleAsync(user, userRole.Role);

            if (!response.Succeeded)
                return BadRequest(new
                {
                    Success = false,
                    Errors = response.Errors.Select(e => e.Description)
                });

            return Ok(new
            {
                Success = true,
                Message = $"User '{user.UserName}' '{userRole.Role}' priviledges have been revoked."
            });
        }
    }
}

