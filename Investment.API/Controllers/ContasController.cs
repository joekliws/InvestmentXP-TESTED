using Investment.Domain.DTOs;
using Investment.Domain.Helpers;
using Investment.Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investment.API.Controllers
{
    [Route("conta")]
    [ApiController]
    [Authorize]
    public class ContasController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly IAuthService _auth;

        public ContasController(IAccountService service, IAuthService auth)
        {
            _service = service;
            _auth = auth;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AccountReadDTO>> CreateAccount (AccountCreateDTO request)
        {
            string url = $@"{Request.Scheme}://{Request.Host}";
            var response = await _service.CreateAccount(request);
            return Created(url, response);
        }

        [HttpGet("{cod-cliente}")]
        public async Task<ActionResult> GetCustomerById ()
        {
            int.TryParse(Request.RouteValues["cod-cliente"].ToString(), out int customerId);
            string token = Request.Headers.Authorization.ToString().Split(' ')[1];

            _auth.ValidateToken(token);
            _auth.CheckTokenBelongsToUser(token, customerId);

            var response = await _service.GetBalance(customerId);
            return Ok(response);
        }
        
        [HttpPost("deposito")]
        public async Task<ActionResult> Deposit(Operation operation)
        {
            string token = Request.Headers.Authorization.ToString().Split(' ')[1];
            _auth.ValidateToken(token);
            _auth.CheckTokenBelongsToUser(token, operation.CodCliente);

            await _service.Deposit(operation);
            return Ok(new {message = "Operação realizada com sucesso"});
        }

        [HttpPost("saque")]
        public async Task<ActionResult> Withdraw(Operation operation)
        {
            string token = Request.Headers.Authorization.ToString().Split(' ')[1];
            _auth.ValidateToken(token);
            _auth.CheckTokenBelongsToUser(token, operation.CodCliente);

            await _service.Withdraw(operation);
            return Ok(new { message = "Operação realizada com sucesso" });
        }
    }
}
