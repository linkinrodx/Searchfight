using Searchfight.Proxy.Contracts;
using Searchfight.Proxy.Models;
using System;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;

namespace Searchfight.Proxy.Services
{
    public class GoogleSearchService : IGoogleSearchService
    {
        readonly HttpClient _Client;
        readonly string name = "Google";
        readonly string GoogleKey = "";
        readonly string GoogleCX = "";
        readonly string GoogleUri = "https://www.googleapis.com/customsearch/v1?key={0}&cx={1}&q={2}";

        public GoogleSearchService()
        {
            _Client = new HttpClient();
        }

        public async Task<GoogleSearchResponse> Search(string query)
        {
            var result = new GoogleSearchResponse();
            var endpoint = string.Format(GoogleUri, GoogleKey, GoogleCX, query);

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
            return name;
        }
    }
}
