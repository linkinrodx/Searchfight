using System;
using System.Collections.Generic;
using System.Text;

namespace Searchfight.Proxy.Models
{
    public class Query
    {
        public Query()
        {
            ListResult = new List<ResultQuery>();
        }

        public string Description { get; set; }
        public List<ResultQuery> ListResult { get; set; }
    }
}
