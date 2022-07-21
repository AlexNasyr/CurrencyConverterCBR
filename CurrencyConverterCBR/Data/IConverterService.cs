using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverterCBR.Data {
    public interface IConverterService {
        Task<List<ValutaItem>> GetCurrencies();
        Task<List<ValCursValute>> GetCurrenciesRates();
    }
}
