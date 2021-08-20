using CurrencyExchange.API.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.API.Services.Interfaces
{
    public interface ICurrencyPurchaseService
    {
        Task<CurrencyPurchaseResponseDto> SavePurchaseAsync(CurrencyPurchaseRequestDto request);
    }

}
