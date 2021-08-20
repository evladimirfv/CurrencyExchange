using System.Threading.Tasks;
using CurrencyExchange.API.Dto;
using CurrencyExchange.API.Services;

namespace CurrencyExchange.API.Services.Helpers
{
    public interface IRate
    {
        Task<ExchangeRateResponseDto> GetRateAsync();
    }

    public abstract class BaseRate
    {
        protected readonly ApiSettings Settings;
        protected readonly IHttpCallService HttpService;

        protected BaseRate(ApiSettings settings, IHttpCallService httpService)
        {
            Settings = settings;
            HttpService = httpService;
        }
    }
}
