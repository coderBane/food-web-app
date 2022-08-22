using AutoMapper;
using Foody.Auth.DTOs;
using Foody.Auth.Models;
using Foody.Data.Interfaces;
using Foody.Auth.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace Foody.WebApi.Controllers.v1
{
    public class AccountController : BaseController
    {
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(IUnitofWork unitofWork, IMapper mapper, UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
            : base(unitofWork, mapper)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        // v1/Account/Register
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(200, Type = typeof(AuthResult))]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            if (!registrationDto.Password.Equals(registrationDto.ConfirmPassword))
                return BadRequest(new AuthResult()
                { Success = false, Errors = new List<string> { "Passwords do not match!" } });

            var user = await _userManager.FindByEmailAsync(registrationDto.Email);

            if (user is not null)
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "Email already in Use.",
                        "User Already Exists",
                    }
                });

            string uName = string.IsNullOrEmpty(registrationDto.UserName) ?
                registrationDto.Email.Split('@').ElementAt(0) : registrationDto.UserName;

            var newUser = new IdentityUser()
            {
                Email = registrationDto.Email,
                UserName = uName,
                EmailConfirmed = true, // TODO build confirmation functionality
            };

            var created = await _userManager.CreateAsync(newUser, registrationDto.Password);

            if (!created.Succeeded) // if registrarion fails
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = created.Errors.Select(x => x.Description).ToList(),
                });

            // Generate jwt token
            var token = JwtToken(newUser);

            return Ok(new AuthResult()
            {
                Token = token,
                Success = true,
            });
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(200, Type = typeof(AuthResult))]
        public async Task<IActionResult> LogIn([FromBody] LogInDto logInDto)
        {
            var exists = await _userManager.FindByEmailAsync(logInDto.Email) ??
                await _userManager.FindByNameAsync(logInDto.Email);

            if (exists is null)
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "Invalid Authentication Request."
                    }
                });

            var correctPass = await _userManager.CheckPasswordAsync(exists, logInDto.Password);

            if (!correctPass)
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "Invalid Authentication Request."
                    }
                });

            var jwtToken = JwtToken(exists);

            return Ok(new AuthResult()
            {
                Token = jwtToken,
                Success = true
            });
        }

        private string JwtToken(IdentityUser user)
        {
            // responsiple for creating the token
            var handler = new JwtSecurityTokenHandler();

            // the secret key
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            // information to create token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    // used by the refresh token
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2), // TODO Update to 2 minutes
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // generate token
            var token = handler.CreateToken(tokenDescriptor);

            // convert token into string
            var jwtToken = handler.WriteToken(token);

            return jwtToken;
        }
    }
}

