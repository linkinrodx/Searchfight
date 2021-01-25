using Microsoft.Extensions.Configuration;
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
        readonly HttpClient _Client;

        readonly string Name;
        readonly string Key;
        readonly string Uri;

        public BingSearchService(IConfiguration configuration)
        {
            Name = configuration.GetSection("Providers:Bing:Name").Value;
            Key = configuration.GetSection("Providers:Bing:Key").Value;
            Uri = configuration.GetSection("Providers:Bing:Uri").Value;

            _Client = new HttpClient()
            {
                DefaultRequestHeaders = { { "Ocp-Apim-Subscription-Key", Key } }
            };
        }

        public async Task<BingSearchResponse> Search(string query)
        {
            var result = new BingSearchResponse();
            var endpoint = string.Format(Uri, query);

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
            return Name;
        }
    }
}
