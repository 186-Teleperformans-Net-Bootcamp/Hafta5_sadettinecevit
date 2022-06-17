using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Application.Dto;
using SocialNetwork.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialNetwork.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<User> userManager
            , IConfiguration configuration, RoleManager<IdentityRole> roleManager
            , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserDTO model)
        {
            IActionResult retVal = null;
            List<Claim>? claims = new List<Claim>();
            User user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                retVal = BadRequest();
            }
            else
            {
                bool result = await _signInManager.CanSignInAsync(user);
                if (result)
                    result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!result)
                {
                    retVal = BadRequest();
                }
                else
                {
                    await _signInManager.SignInAsync(user, result);
                    if (result)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Any())
                        {
                            foreach (var role in roles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));
                            }
                            claims.Add(new Claim(ClaimTypes.Name, model.Email));
                            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        }

                        JwtSecurityToken token = null;

                        if (claims.Any())
                        {
                            token = GetToken(claims);
                        }
                        else
                        {
                            token = GetToken();
                        }

                        var handler = new JwtSecurityTokenHandler();
                        string jwt = handler.WriteToken(token);

                        retVal = Ok(new
                        {
                            token = jwt,
                            expiration = token.ValidTo
                        });
                    }
                }
            }

            return retVal;
        }

        private JwtSecurityToken GetToken(List<Claim> claims = null)
        {
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            JwtSecurityToken token = null;

            if (claims != null)
            {
                token = new JwtSecurityToken(
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                    issuer: _configuration["JWT: Issuer"],
                    audience: _configuration["JWT: Audience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: claims
                    );
            }
            else
            {
                token = new JwtSecurityToken(
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                    issuer: _configuration["JWT: Issuer"],
                    audience: _configuration["JWT: Audience"],
                    expires: DateTime.Now.AddDays(1)
                    );
            }
            return token;
        }

    }
}
