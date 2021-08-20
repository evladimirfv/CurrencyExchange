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
    [Route("api/exchangerate")]
    [EnableCors("api-currencyexchange-policy")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly ApiSettings _settings;
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController(IExchangeRateService rateService, IOptions<ApiSettings> settings)
        {
            _exchangeRateService = rateService;
            _settings = settings.Value;
        }

        [HttpGet("rate/{iso}", Name = "GetCurrencyRate")]
        [ProducesResponseType(typeof(ExchangeRateResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrencyRateAsync(string iso)
        {
            var isoCodes = _settings
                .ValidCoins
                .Select(x => x.Iso)
                .ToList();

            if (!isoCodes.Contains(iso.ToUpper()))
            {
                throw new BadRequestException($"The supported currencies are : {string.Join(", ", isoCodes)}");
            }

            var response = await _exchangeRateService.GetExchangeRateAsync(iso);
            return Ok(response);
        }

    }
}
