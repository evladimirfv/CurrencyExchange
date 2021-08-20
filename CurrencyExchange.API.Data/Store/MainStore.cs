using System.Threading.Tasks;
using CurrencyExchange.API.Data;

namespace CurrencyExchange.API.Data.Store
{
    public interface IStore
    {
        Task<int> Commit();
    }

    public class MainStore : IStore
    {
        protected readonly DatabaseContext Context;

        public MainStore(DatabaseContext context)
        {
            Context = context;
        }

        public async Task<int> Commit()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
