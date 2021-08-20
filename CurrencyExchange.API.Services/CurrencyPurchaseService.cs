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
    public class CurrencyPurchaseService: ICurrencyPurchaseService
    {
        private readonly IExchangeRateService _rateService;
        private readonly IPurchaseStore _purchaseStore;
        private readonly IUserStore _userStore;
        private readonly ApiSettings _settings;

        public CurrencyPurchaseService(IExchangeRateService exchangeRateService, IUserStore userStore, IPurchaseStore purchaseStore, IOptions<ApiSettings> settings)
        {
            _rateService = exchangeRateService;
            _purchaseStore = purchaseStore;
            _settings = settings.Value;
            _userStore = userStore;
        }

        public async Task<CurrencyPurchaseResponseDto> SavePurchaseAsync(CurrencyPurchaseRequestDto request)
        {
            var user = _userStore.Users.FirstOrDefault(x => x.UserId == request.UserId);

            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            var rate = await _rateService.GetExchangeRateAsync(request.DestinationCurrency);
            var currencyInfo = _settings.ValidCoins.FirstOrDefault(x => x.Iso == request.DestinationCurrency.ToUpper());
            decimal estimatedTargetAmount = Math.Round(request.OriginalValue / rate.Selling, 2);
            decimal totalInMonth = await _purchaseStore.GetTotalMonthPurchasesAsync(request.UserId, request.DestinationCurrency.ToUpper());

            if (estimatedTargetAmount + totalInMonth > currencyInfo.Max)
            {
                throw new BadRequestException(
                    "Exchange amount error",
                    new Dictionary<string, string[]>
                    {
                        {"Max Limit has been exceeded", new []
                        {
                            $"Current buy {estimatedTargetAmount} {currencyInfo.Iso}",
                            $"Available buy {currencyInfo.Max - totalInMonth} {currencyInfo.Iso}",
                            $"The current month limit of {currencyInfo.Iso} available to purchase is {currencyInfo.Max}",
                            $"Current rate is {rate.Selling} ARS per {currencyInfo.Iso}",
                            $"Available for exchange {Math.Round((currencyInfo.Max - totalInMonth) * rate.Selling, 2)} ARS."
                        }},
                    },
                    "The currency amount to purchase exceeds the monthly limit");
            }

            var purchase = new Purchase
            {
                UserId = request.UserId,
                OriginAmount = request.OriginalValue,
                TargetAmount = estimatedTargetAmount,
                TargetCurrency = request.DestinationCurrency.ToUpper(),
            };

            await _purchaseStore.Purchases.AddAsync(purchase);
            int response = await _purchaseStore.CommitAsync();

            if (response == 0)
            {
                throw new Exception("Error on save");
            }

            return new CurrencyPurchaseResponseDto
            {
                Currency = currencyInfo.Iso,
                PurchasedValue = estimatedTargetAmount
            };
        }
    }
}
