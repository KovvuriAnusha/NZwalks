using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequest)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.UserName
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerRequest.Password);
            if (identityResult.Succeeded)
            {
                // Add roles to this user
                if (registerRequest.Roles != null && registerRequest.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequest.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered");
                    }
                }
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.userName);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequest.password);
                if (checkPasswordResult)
                {
                    //Get roles for user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDetail() { jwtToken = jwtToken };
                        return Ok(response);
                    }
                    // Create Token
                    return BadRequest("Username and password is incorrect");
                }
            }
            return BadRequest("Username and password is incorrect");
        }
    }
}
