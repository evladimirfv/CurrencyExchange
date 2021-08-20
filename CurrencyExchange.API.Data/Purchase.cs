using System;

namespace CurrencyExchange.API.Data
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public decimal OriginAmount { get; set; }
        public string OriginCurrency { get; set; } = "ARS";
        public decimal TargetAmount { get; set; }
        public string TargetCurrency { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now.ToUniversalTime();

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
