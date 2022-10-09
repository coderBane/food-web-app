using AutoMapper;

using Foody.Auth.DTOs;
using Foody.Auth.Models;
using Foody.Entities.Models;
using Foody.Data.Interfaces;
using Foody.Auth.Configuration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Foody.Data.Services;

namespace Foody.WebApi.Controllers.v1
{
    public sealed class AccountController : BaseController
    {
        private readonly JwtConfig _jwtConfig;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly TokenValidationParameters _tokenValidationParameters;

        public AccountController(IUnitofWork unitofWork, IMapper mapper, ICacheService cacheService,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParameters)
            : base(unitofWork, mapper, cacheService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParameters = tokenValidationParameters;
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

            // check if email already exists
            var user = await _userManager.FindByEmailAsync(registrationDto.Email);

            if (user is not null)
                return Conflict(new AuthResult()
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

            // create user
            var validUser = new IdentityUser()
            {
                Email = registrationDto.Email,
                UserName = uName,
                EmailConfirmed = true, // TODO build confirmation functionality
            };

            var created = await _userManager.CreateAsync(validUser, registrationDto.Password);

            if (!created.Succeeded) // if registrarion fails
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = created.Errors.Select(x => x.Description).ToList(),
                });

            // Default role
            await _userManager.AddToRoleAsync(validUser, "User");

            // Generate jwt token
            //var token = await JwtToken(validUser);

            return Ok(await JwtToken(validUser));
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(200, Type = typeof(AuthResult))]
        public async Task<IActionResult> LogIn([FromBody] LogInDto logInDto)
        {
            var exists = await _userManager.FindByEmailAsync(logInDto.Email) ??
                await _userManager.FindByNameAsync(logInDto.Email);

            if (exists is null) // email/username not in database
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

            //var token = await JwtToken(exists);

            return Ok(await JwtToken(exists));
        }

        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(200, Type = typeof(AuthResult))]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequestDto)
        {
            // check if token is valid
            var result = await VerifyToken(tokenRequestDto);

            if (result is null)
                return BadRequest(new AuthResult()
                {
                    Success = false,
                    Errors = new List<string>
                    {
                        "Token validation failure."
                    }
                });

            return !result.Errors.Any() ? Ok(result) : BadRequest(result);
        }

        private async Task<AuthResult?> VerifyToken(TokenRequestDto tokenRequestDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(tokenRequestDto.Token, _tokenValidationParameters, out var validatedToken);

                // validate token
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    // check is algorithms match
                    var result = jwtSecurityToken.Header.Alg
                        .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!result) return null;
                }

                long utcExpiration = long.Parse(principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)!.Value);

                var expDate = DateTimeOffset.FromUnixTimeSeconds(utcExpiration).UtcDateTime;

                // check if jwt token has expired
                if (expDate > DateTime.UtcNow)
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Jwt Token has not expired.",
                            "Cannot generate new token(s)."
                        }
                    };

                var storedToken = _unitofWork.RefreshToken.Find(rt => rt.Token == tokenRequestDto.RefreshToken)
                    .AsParallel().FirstOrDefault();

                if (storedToken is null)
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Invalid Token."
                        }
                    };

                if (storedToken.IsUsed)
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Invalid Token. Used!"
                        }
                    };

                if (storedToken.IsRevoked)
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Invalid Token. Revoked!"
                        }
                    };

                if (storedToken.ExipryDate < DateTime.UtcNow)
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Token has expired."
                        }
                    };

                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;

                if (storedToken.JwtId != jti)
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Invalid Token."
                        }
                    };

                // start processing
                bool updated = await _unitofWork.RefreshToken.Update(storedToken);

                if (!updated) return null;
       
                await _unitofWork.CompleteAsync();

                // regenerate token
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

                if (dbUser is null)
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Error Processing Request."
                        }
                    };

                return await JwtToken(dbUser);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null; // TODO implement errror handling
            }
        }

        private async Task<AuthResult> JwtToken(IdentityUser user)
        {
            // responsiple for creating the token
            var handler = new JwtSecurityTokenHandler();

            // the secret key
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            // get all the user claims
            var claims = await ValidClaims(user);

            // information to create token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // generate token
            var token = handler.CreateToken(tokenDescriptor);

            // convert token into string
            var jwtToken = handler.WriteToken(token);

            // generate refresh token
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = $"{RandomString(25)}-{Guid.NewGuid()}",
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                ExipryDate = DateTime.UtcNow.AddMonths(6),
            };

            await _unitofWork.RefreshToken.Add(refreshToken);
            await _unitofWork.CompleteAsync();

            //var tokenData = new TokenData
            //{
            //    JwtToken = jwtToken,
            //    RefreshToken = refreshToken.Token
            //};

            return new AuthResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Success = true,
            };
        }

        private async Task<List<Claim>> ValidClaims(IdentityUser user)
        {
            var options = new IdentityOptions();

            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // used by the refresh token
            };

            // Add user claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Add user roles to claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role is null) continue;

                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }

        private static readonly Random seed = new();

        private static string RandomString(int lenght)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789-@+.";

            lock (seed)
            {
                return new string(Enumerable.Repeat(chars, lenght)
                .Select(s => s[seed.Next(s.Length)]).ToArray());
            }
        }
    }
}

