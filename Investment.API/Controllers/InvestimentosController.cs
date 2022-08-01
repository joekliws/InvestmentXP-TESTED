using Investment.Domain.DTOs;
using Investment.Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investment.API.Controllers
{
    [Route("investimentos")]
    [ApiController]
    [Authorize]
    public class InvestimentosController : ControllerBase
    {
        private readonly IAssetService _service;
        private readonly IAuthService _auth;

        public InvestimentosController(IAssetService service, IAuthService auth)
        {
            _service = service;
            _auth = auth;
        }

        [HttpPost("comprar")]
        public async Task<ActionResult> BuyAsset(AssetCreateDTO asset)
        {
            string token = Request.Headers.Authorization.ToString().Split(' ')[1];
            _auth.ValidateToken(token);
            _auth.CheckTokenBelongsToUser(token, asset.CodCliente);

            await _service.Buy(asset);
            return Ok(new {message = "Compra efetuada com sucesso"});
        }

        [HttpPost("vender")]
        public async Task<ActionResult> SellAsset(AssetCreateDTO asset)
        {
            string token = Request.Headers.Authorization.ToString().Split(' ')[1];
            _auth.ValidateToken(token);
            _auth.CheckTokenBelongsToUser(token, asset.CodCliente);

            await _service.Sell(asset);
            return Ok(new { message = "Venda efetuada com sucesso" });

        }
    }
}
