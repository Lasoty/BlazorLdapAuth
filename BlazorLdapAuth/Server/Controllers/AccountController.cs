using BlazorLdapAuth.Server.Authentication;
using BlazorLdapAuth.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorLdapAuth.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        public AccountController(JwtAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var userSession = jwtAuthenticationManager.GenerateJwtToken(request.UserName, request.Password);
            return userSession == null ? Unauthorized() : Ok(userSession);
        }
    }
}
