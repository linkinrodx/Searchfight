using Newtonsoft.Json;
using System;

namespace Searchfight.Proxy.Models
{
    public class BingWebPagesSearchResponse
    {
        [JsonProperty(PropertyName = "totalEstimatedMatches")]
        public long TotalResults { get; set; }
    }
}
