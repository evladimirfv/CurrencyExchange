using System;
using System.Globalization;
using System.Threading.Tasks;
using CurrencyExchange.API.Dto;
using CurrencyExchange.API.Services;

namespace CurrencyExchange.API.Services.Helpers
{
    public class BrlRate : BaseRate, IRate
    {
        public BrlRate(ApiSettings settings, IHttpCallService httpService) : base(settings, httpService) { }

        public async Task<ExchangeRateResponseDto> GetRateAsync()
        {
            var response = await HttpService.CallEndpoint(HttpVerb.Get, new Uri(Settings.BRL_Url));
            return ParseBrlRate(response);
        }

        private ExchangeRateResponseDto ParseBrlRate(string response)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var parts = response
                .Replace("[", "")
                .Replace("]", "")
                .Replace("\"", "")
                .Split(',');

            var rateResponse = new ExchangeRateResponseDto
            {
                Buying = decimal.Parse(parts[0]) / 4,
                Selling = decimal.Parse(parts[1]) / 4,
                ExchangeRateDate = DateTime.ParseExact(parts[2].Replace("Actualizada al ", ""), "dd/M/yyyy HH:mm", provider)
            };

            return rateResponse;

        }
    }
}
