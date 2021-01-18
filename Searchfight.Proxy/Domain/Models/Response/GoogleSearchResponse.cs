using Newtonsoft.Json;

namespace Searchfight.Proxy.Models
{
    public class GoogleSearchResponse
    {
        [JsonProperty(PropertyName = "searchInformation")]
        public GoogleSearchInformationResponse Information { get; set; }
    }
}
