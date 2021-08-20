using CurrencyExchange.API.Data;
using CurrencyExchange.API.Data.Store;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.API.Data.Store
{
    public class UserStore : MainStore, IUserStore
    {
        public UserStore(DatabaseContext context) : base(context) { }
        public DbSet<User> Users => Context.Users;
    }
}
