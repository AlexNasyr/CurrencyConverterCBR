using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CurrencyConverterCBR.Data {
    public class ConverterService : IConverterService {
        private readonly ILogger _logger;
        public ConverterService(ILogger logger) { 
            _logger = logger;
        }

        public async Task<List<ValutaItem>> GetCurrencies() {
            string currenciesXML = await Get(@"http://www.cbr.ru/scripts/XML_val.asp?d=0");

            XmlSerializer xmlSerializer = new(typeof(Valuta));
            Valuta curr = (Valuta)xmlSerializer.Deserialize(new StringReader(currenciesXML));
            
            return new List<ValutaItem>(curr.Item);
        }
        public async Task<List<ValCursValute>> GetCurrenciesRates() {
            string currenciesXML = await Get(@"http://www.cbr.ru/scripts/XML_daily.asp"); //?date_req=02/03/2002

            XmlSerializer xmlSerializer = new(typeof(ValCurs));
            ValCurs rates = (ValCurs)xmlSerializer.Deserialize(new StringReader(currenciesXML));

            return new List<ValCursValute>(rates.Valute);
        }

        private async Task<string> Get(string url) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("windows-1251");

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            byte[] bytes = await response.Content.ReadAsByteArrayAsync();
                       
            string responseString = encoding.GetString(bytes, 0, bytes.Length);
            
            return responseString;
        }
    }
}
