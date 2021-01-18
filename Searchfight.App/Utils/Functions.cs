using Searchfight.Proxy.Models;
using System.Collections.Generic;
using System.Linq;

namespace Searchfight.App.Utils
{
    public class Functions
    {
        public static bool ValidParams(string[] QueryParams, out List<string> NewQueryParams)
        {
            var QuoteCount = 0;
            NewQueryParams = new List<string>();

            if (QueryParams.Length <= 0)
            {
                return false;
            }

            for (int i = 0; i < QueryParams.Length; i++)
            {
                if (QueryParams[i].Contains('"'))
                {
                    if (QuoteCount % 2 == 0)
                    {
                        NewQueryParams.Add(QueryParams[i]);
                    }
                    else
                    {
                        NewQueryParams[^1] = NewQueryParams[^1] + " " + QueryParams[i];
                    }

                    QuoteCount++;
                }
                else
                {
                    var param = QueryParams[i].Replace('\"', '\0');

                    if (QuoteCount % 2 == 0)
                    {
                        NewQueryParams.Add(param);
                    }
                    else
                    {
                        NewQueryParams[^1] = NewQueryParams[^1] + " " + param;
                    }
                }
            }

            var valid = QuoteCount % 2 == 0;

            foreach (var item in NewQueryParams)
            {
                item.Replace('\"', ' ');
                item.Trim();
            }

            return valid;
        }

        public static string GetMaxByProvider(List<Query> queries, int providerId)
        {
            var result = string.Empty;

            var listProviderResult = (
                from q in queries
                select new { q.Description, q.ListResult.Where(o => o.ProviderId == providerId).FirstOrDefault().Amount }
            ).OrderByDescending(o => o.Amount);

            if (listProviderResult.FirstOrDefault() != null)
            {
                result = listProviderResult.FirstOrDefault().Description;
            }

            return result;
        }

        public static string SumByProvider(List<Query> queries)
        {
            var result = string.Empty;

            var listProviderResult = (
                from q in queries
                select new { q.Description, amount = q.ListResult.Sum(o => o.Amount) }
            ).OrderByDescending(o => o.amount);

            if (listProviderResult.FirstOrDefault() != null)
            {
                result = listProviderResult.FirstOrDefault().Description;
            }

            return result;
        }
    }
}
