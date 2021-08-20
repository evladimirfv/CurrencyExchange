using System.Collections.Generic;

namespace CurrencyExchange.API.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }

        public List<Purchase> Purchases { get; set; } = new();
    }
}
