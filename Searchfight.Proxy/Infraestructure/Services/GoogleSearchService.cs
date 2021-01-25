using Searchfight.Proxy.Contracts;
using Searchfight.Proxy.Models;
using System;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Searchfight.Proxy.Services
{
    public class GoogleSearchService : IGoogleSearchService
    {
        readonly HttpClient _Client;
        readonly string Name;
        readonly string Key;
        readonly string CX;
        readonly string Uri;

        public GoogleSearchService(IConfiguration configuration)
        {
            _Client = new HttpClient();
            Name = configuration.GetSection("Providers:Google:Name").Value;
            Key = configuration.GetSection("Providers:Google:Key").Value;
            CX = configuration.GetSection("Providers:Google:CX").Value;
            Uri = configuration.GetSection("Providers:Google:Uri").Value;
        }

        public async Task<GoogleSearchResponse> Search(string query)
        {
            var result = new GoogleSearchResponse();
            var endpoint = string.Format(Uri, Key, CX, query);

            try
            {
                var response = await _Client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var googleResult = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<GoogleSearchResponse>(googleResult);
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
