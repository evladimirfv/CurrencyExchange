using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.API.Services.Helpers
{ 
    public class ApiSettings
    {
        public string USD_Url { get; set; }
        public string BRL_Url { get; set; }
        public string CAD_Url { get; set; }
        public ValidCoins[] ValidCoins { get; set; }
    }

    public class ValidCoins    {
        public string Iso { get; set; }
        public decimal Max { get; set; }
    }
}

