using Newtonsoft.Json;
using Searchfight.Proxy.Contracts;
using Searchfight.Proxy.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Searchfight.Proxy.Services
{
    public class BingSearchService : IBingSearchService
    {
        HttpClient _Client;
        readonly string name = "";
        readonly string BingKey = "";
        readonly string BingUri = "https://api.bing.microsoft.com/v7.0/search?q={0}";

        public BingSearchService()
        {
            _Client = new HttpClient()
            {
                DefaultRequestHeaders = { { "Ocp-Apim-Subscription-Key", BingKey } }
            };
        }

        public async Task<BingSearchResponse> Search(string query)
        {
            var result = new BingSearchResponse();
            var endpoint = string.Format(BingUri, query);

            try
            {
                var response = await _Client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var bingResult = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<BingSearchResponse>(bingResult);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ProviderName()
        {
            return name;
        }
    }
}
