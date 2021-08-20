using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.API.Dto
{
    public class CurrencyPurchaseRequestDto
    {
        public int UserId { get; set; }
        public decimal OriginalValue { get; set; }
        public string DestinationCurrency { get; set; }
    }

}
