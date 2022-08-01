using Investment.Domain.DTOs;
using Investment.Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investment.API.Controllers
{
    [Route("ativos")]
    [ApiController]
    [Authorize]
    public class AtivosController : ControllerBase
    {
        private readonly IAssetService _service;
        private readonly IAuthService _auth;

        public AtivosController(IAssetService service, IAuthService auth)
        {
            _service = service;
            _auth = auth;
        }

        [HttpGet("cliente/{cod-cliente}")]
        public async Task<ActionResult<IEnumerable<CustomerAssetReadDTO>>> GetAssetsByCustomer()
        {
            int.TryParse(Request.RouteValues["cod-cliente"].ToString(), out int customerId);
            string token = Request.Headers.Authorization.ToString().Split(' ')[1];
            _auth.ValidateToken(token);
            _auth.CheckTokenBelongsToUser(token, customerId);

            var response = await _service.GetAssetsByCustomer(customerId);
            return Ok(response);
        }

        [HttpGet("{cod-ativo}")]
        public async Task<ActionResult<AssetReadDTO>> GetAssetsById()
        {
            string token = Request.Headers.Authorization.ToString().Split(' ')[1];
            _auth.ValidateToken(token);

            int.TryParse(Request.RouteValues["cod-ativo"].ToString(), out int assetId);
            var response = await _service.GetAssetById(assetId);
            return Ok(response);
        }



    }
}
