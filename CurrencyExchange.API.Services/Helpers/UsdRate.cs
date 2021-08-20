using System;
using System.Globalization;
using System.Threading.Tasks;
using CurrencyExchange.API.Dto;
using CurrencyExchange.API.Services;
using CurrencyExchange.API.Services.Helpers;

namespace CurrencyExchange.API.Services.Helpers
{
    public class UsdRate : BaseRate, IRate
    {
        public UsdRate(ApiSettings settings, IHttpCallService httpService) : base(settings, httpService)  { }

        public async Task<ExchangeRateResponseDto> GetRateAsync()
        {
            var response = await HttpService.CallEndpoint(HttpVerb.Get, new Uri(Settings.USD_Url));
            return ParseUsdRate(response);
        }

        private ExchangeRateResponseDto ParseUsdRate(string response)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var parts = response
                .Replace("[", "")
                .Replace("]", "")
                .Replace("\"", "")
                .Split(',');

            var rateResponse = new ExchangeRateResponseDto
            {
                Buying = decimal.Parse(parts[0]),
                Selling = decimal.Parse(parts[1]),
                ExchangeRateDate = DateTime.ParseExact(parts[2].Replace("Actualizada al ", ""), "dd/M/yyyy HH:mm", provider)
            };

            return rateResponse;
        }

    }
}
