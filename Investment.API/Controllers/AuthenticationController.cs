using Investment.Domain.Helpers;
using Investment.Infra.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investment.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthenticationController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(AccountLogin login)
        {
            var isLoggedin = await _service.Login(login.UserLogin, login.Password);
            
            if (isLoggedin.Value)
            {
                string token = _service.GenerateToken(isLoggedin.Key);
                return Ok(new {Token = token});
            }
            return Unauthorized();
        }
    }
}
