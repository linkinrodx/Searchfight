using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Searchfight.Proxy.Models
{
    public class BingSearchResponse
    {
        [JsonProperty(PropertyName = "WebPages")]
        public BingWebPagesSearchResponse Information { get; set; }
    }
}
