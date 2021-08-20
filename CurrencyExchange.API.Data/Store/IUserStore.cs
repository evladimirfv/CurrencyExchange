using CurrencyExchange.API.Data;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.API.Data.Store
{
    public interface IUserStore
    {
        DbSet<User> Users { get; }
    }
}
