using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyExchange.API.Services;
using CurrencyExchange.API.Services.Interfaces;
using CurrencyExchange.API.Services.Helpers;
using Microsoft.Extensions.Options;
using CurrencyExchange.API.Dto;
using CurrencyExchange.API.Data;
using CurrencyExchange.API.Data.Store;

namespace CurrencyExchange.API.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ApiSettings _settings;
        private readonly IHttpCallService _httpService;

        public ExchangeRateService(IOptions<ApiSettings> settings, IHttpCallService httpService)
        {
            _settings = settings.Value;
            _httpService = httpService;
        }

        public async Task<ExchangeRateResponseDto> GetExchangeRateAsync(string iso)
        {
            string classTypeString = $"CurrencyExchange.API.Services.Helpers.{iso.FirstCharToUpper()}Rate";
            var type = Type.GetType(classTypeString);
            var constructor = type?.GetConstructor(new[] { typeof(ApiSettings), typeof(HttpCallService) });

            if (constructor != null)
            {
                var retriever = (IRate)constructor.Invoke(new object[] { _settings, _httpService });

                var response = await retriever.GetRateAsync();
                response.Currency = iso;
                response.Buying = Math.Round(response.Buying, 2);
                response.Selling = Math.Round(response.Selling, 2);

                return response;
            }

            throw new Exception("It was not possible to create an instance for the Currency Rate Retriever.");
        }

        
    }

    
}
