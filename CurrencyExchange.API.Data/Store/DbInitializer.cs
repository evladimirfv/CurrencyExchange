using System.Linq;
using CurrencyExchange.API.Data;

namespace CurrencyExchange.API.Data.Store
{
    public static class DbInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
                return;

            context.Users.AddRange(new User[]
            {
                new User { Name = "Virtualmind1"},
                new User { Name = "Virtualmind2"},
                new User { Name = "Virtualmind3"}
            });
            context.SaveChanges();
        }
    }
}
