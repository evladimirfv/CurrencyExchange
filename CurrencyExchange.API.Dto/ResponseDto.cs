using System;

namespace CurrencyExchange.API.Dto
{
    public class CurrencyPurchaseResponseDto
    {
        public string Currency { get; set; }
        public decimal PurchasedValue { get; set; }
    }

    public class ExchangeRateResponseDto
    {
        public string Currency { get; set; }
        public decimal Buying { get; set; }
        public decimal Selling { get; set; }
        public DateTime ExchangeRateDate { get; set; }
    }

}
