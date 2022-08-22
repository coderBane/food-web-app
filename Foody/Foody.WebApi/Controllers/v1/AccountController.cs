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


namespace Foody.WebApi.Controllers.v1
{
    public class AccountController : BaseController
    {
        private readonly JwtConfig _jwtConfig;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly TokenValidationParameters _tokenValidationParameters;

        public AccountController(IUnitofWork unitofWork, IMapper mapper, UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor, TokenValidationParameters tokenValidationParameters)
            : base(unitofWork, mapper)
        {
            _userManager = userManager;
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

            // Generate jwt token
            var token = await JwtToken(validUser);

            return Ok(new AuthResult()
            {
                Token = token.JwtToken,
                RefreshToken = token.RefreshToken,
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

            var token = await JwtToken(exists);

            return Ok(new AuthResult()
            {
                Token = token.JwtToken,
                RefreshToken = token.RefreshToken,
                Success = true
            });
        }

        //[HttpPost]
        //[Route("RefreshToken")]
        //[ProducesResponseType(200, Type = typeof(AuthResult))]
        //public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequestDto)
        //{
        //    // check if token is valid
        //    var result = await VerifyToken(tokenRequestDto);

        //    if (result is null)
        //        return BadRequest(new AuthResult()
        //        {
        //            Success = false,
        //            Errors = new List<string>
        //                {
        //                    "Token validation failure."
        //                }
        //        });

        //    return Ok(result);
        //}

        //private async Task<AuthResult?> VerifyToken(TokenRequestDto tokenRequestDto)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    try
        //    {
        //        // check if token is valid
        //        var principal = tokenHandler.ValidateToken(tokenRequestDto.Token, _tokenValidationParameters, out var validatedToken);

        //        // validate if the string is an actual jwt token and not random.
        //        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        //        {
        //            // check algorithms match
        //            var result = jwtSecurityToken.Header.Alg.
        //                Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase);

        //            if (!result) return null;
        //        }

        //        // check expiry date of token
        //        var utcExpDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        //        var expDate = UnixToDate(utcExpDate);

        //        // check if jwt token has expired
        //        if (expDate > DateTime.UtcNow)
        //            return new AuthResult()
        //            {
        //                Success = false,
        //                Errors = new List<string>
        //                {
        //                    "Jwt Token has not expired."
        //                }
        //            };

        //        var refreshTokenExists = await _unitofWork.RefreshToken.Exists(tokenRequestDto.RefreshToken);

        //        if (refreshTokenExists is null)
        //            return new AuthResult()
        //            {
        //                Success = false,
        //                Errors = new List<string>
        //                {
        //                    "Invalid refresh token."
        //                }
        //            };

        //        // check if refresh token has expired
        //        if (refreshTokenExists.ExipryDate < DateTime.UtcNow)
        //            return new AuthResult()
        //            {
        //                Success = false,
        //                Errors = new List<string>
        //                {
        //                    "Refresh token has expired."
        //                }
        //            };

        //        if (refreshTokenExists.IsUsed)
        //            return new AuthResult()
        //            {
        //                Success = false,
        //                Errors = new List<string>
        //                {
        //                    "Refresh token has already been used."
        //                }
        //            };

        //        if (refreshTokenExists.IsRevoked)
        //            return new AuthResult()
        //            {
        //                Success = false,
        //                Errors = new List<string>
        //                {
        //                    "Refresh token has been revoked! Cannot be used."
        //                }
        //            };

        //        var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        //        if (!refreshTokenExists.JwtId.Equals(jti))
        //            return new AuthResult()
        //            {
        //                Success = false,
        //                Errors = new List<string>
        //                {
        //                    "Refresh token does not match the jwt token."
        //                }
        //            };

        //        refreshTokenExists.IsUsed = true;

        //        var updateResult = await _unitofWork.RefreshToken.MarkTokenAsUsed(refreshTokenExists);

        //        if (updateResult)
        //        {
        //            await _unitofWork.CompleteAsync();

        //            var dbUser = await _userManager.FindByIdAsync(refreshTokenExists.UserId);

        //            if (dbUser is null)
        //                return new AuthResult()
        //                {
        //                    Success = false,
        //                    Errors = new List<string>
        //                {
        //                    "Error processing request."
        //                }
        //                };

        //            var token = await JwtToken(dbUser);

        //            return new AuthResult()
        //            {
        //                Token = token.JwtToken,
        //                RefreshToken = token.RefreshToken,
        //                Success = true,
        //            };
        //        }

        //        return new AuthResult()
        //        {
        //            Success = false,
        //            Errors = new List<string>
        //                {
        //                    "Error processing."
        //                }
        //        };
        //    }
        //    catch(Exception)
        //    {
        //        return null; // TODO implement error handling
        //    }
        //}

        private DateTime UnixToDate(long unixEpoch)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixEpoch).ToUniversalTime();
            return dateTime;
        }

        private async Task<TokenData> JwtToken(IdentityUser user)
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
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.Ticks.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
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

            _unitofWork.RefreshToken.Add(refreshToken);
            await _unitofWork.CompleteAsync();

            var tokenData = new TokenData
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return tokenData;
        }

        private static readonly Random seed = new();

        private static string RandomString(int lenght)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789-!#&@?+";

            lock (seed)
            {
                return new string(Enumerable.Repeat(chars, lenght)
                .Select(s => s[seed.Next(s.Length)]).ToArray());
            }
        }
    }
}

