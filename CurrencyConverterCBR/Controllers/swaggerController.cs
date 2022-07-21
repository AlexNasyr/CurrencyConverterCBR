using CurrencyConverterCBR.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverterCBR.Controllers {
    public class swaggerController {
        IConverterService _service;
        public swaggerController(IConverterService service) {
            _service = service;
        }

        [HttpGet]
        [Route("swagger/GetCurrencies")]
        public async Task<List<ValutaItem>> GetCurrencies() {
            var responce = await _service.GetCurrencies();
            return responce;
        }

        [HttpGet]
        [Route("swagger/GetCurrenciesRates")]
        public async Task<List<ValCursValute>> GetCurrenciesRates() {
            var responce = await _service.GetCurrenciesRates();
            return responce;
        }
    }
}
