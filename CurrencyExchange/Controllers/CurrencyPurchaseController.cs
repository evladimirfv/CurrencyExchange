using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExchange.API.Services.Helpers;
using CurrencyExchange.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using CurrencyExchange.API.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("api/currencypurchase")]
    [EnableCors("api-currencyexchange-policy")]
    public class CurrencyPurchaseController: ControllerBase
    {
        private readonly ApiSettings _settings;
        private readonly ICurrencyPurchaseService _service;

        public CurrencyPurchaseController(ICurrencyPurchaseService service, IOptions<ApiSettings> settings)
        {
            _service = service;
            _settings = settings.Value;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CurrencyPurchaseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PurchaseAsync([FromBody] CurrencyPurchaseRequestDto request)
        {
            var iso = _settings
                .ValidCoins
                .Select(x => x.Iso)
                .ToList();

            if (!iso.Contains(request.DestinationCurrency.ToUpper()))
            {
                throw new BadRequestException($"API currency not suported");
            }

            if (request.OriginalValue <= 0)
            {
                throw new BadRequestException("The amount to purchase must be greater than zero");
            }

            var purchase = await _service.SavePurchaseAsync(request);

            return Ok(purchase);
        }
    }
}
