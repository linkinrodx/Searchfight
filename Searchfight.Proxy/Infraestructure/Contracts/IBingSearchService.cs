using Searchfight.Proxy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Searchfight.Proxy.Contracts
{
    public interface IBingSearchService
    {
        Task<BingSearchResponse> Search(string query);
        string ProviderName();
    }
}
