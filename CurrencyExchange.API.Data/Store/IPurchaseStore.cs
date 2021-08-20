using System.Threading.Tasks;
using CurrencyExchange.API.Data;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.API.Data.Store
{
    public interface IPurchaseStore
    {
        DbSet<Purchase> Purchases { get; }
        Task<int> CommitAsync();
        Task<decimal> GetTotalMonthPurchasesAsync(int userId, string currency);
    }
}
