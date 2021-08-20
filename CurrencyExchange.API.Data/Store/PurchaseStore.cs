using System;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExchange.API.Data;
using CurrencyExchange.API.Data.Store;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.API.Data.Store
{
    public class PurchaseStore : MainStore, IPurchaseStore
    {
        public PurchaseStore(DatabaseContext context) : base(context) { }

        public DbSet<Purchase> Purchases => Context.Purchases;

        public async Task<decimal> GetTotalMonthPurchasesAsync(int userId, string currency)
        {
            var total = await Purchases
                .Where(x => x.UserId == userId && x.TransactionDate > DateTime.Now.AddMonths(-1) &&
                            x.TargetCurrency == currency)
                .SumAsync(x => x.TargetAmount);

            return total;
        }

        public async Task<int> CommitAsync()
        {
            return await Commit();
        }
    }
}
