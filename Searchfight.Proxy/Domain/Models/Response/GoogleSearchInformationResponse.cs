using Newtonsoft.Json;
using System;

namespace Searchfight.Proxy.Models
{
    public class GoogleSearchInformationResponse
    {
        [JsonProperty(PropertyName = "totalResults")]
        public long TotalResults { get; set; }
    }
}
