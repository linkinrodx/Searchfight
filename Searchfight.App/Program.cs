using Microsoft.Extensions.DependencyInjection;
using Searchfight.App.Utils;
using Searchfight.Proxy.Contracts;
using Searchfight.Proxy.Models;
using Searchfight.Proxy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Searchfight.Proxy.Utils;

namespace Searchfight.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                args = new string[] { ".net", "java"};

                var provider = ServiceCollection(ConfigurationManager.AppSetting);

                var NewArgs = new List<string>();

                if (!Functions.ValidParams(args, out NewArgs))
                {
                    Console.WriteLine();
                    Console.WriteLine("Bad query params.");

                    return;
                }

                await Search(provider, NewArgs);
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                Console.WriteLine();
                Console.WriteLine("There is a problem...");
                Console.WriteLine();
                Console.WriteLine(message);
            }
        }

        #region Methods
        static async Task Search(ServiceProvider provider, List<string> NewArgs)
        {
            var googleService = provider.GetRequiredService<IGoogleSearchService>();
            var bingService = provider.GetRequiredService<IBingSearchService>();

            var ListQuery = new List<Query>();
            var ListProviders = new List<Provider>
                {
                    new Provider() { Id = (int)ProviderEnum.Google, Description = "Google" },
                    new Provider() { Id = (int)ProviderEnum.Bing, Description = "Bing" }
                };

            foreach (var item in NewArgs)
            {
                var query = new Query
                {
                    Description = item
                };

                var googleResult = await googleService.Search(query.Description);
                var resultQuery = new ResultQuery
                {
                    ProviderId = (int)ProviderEnum.Google,
                    Amount = googleResult.Information.TotalResults
                };
                query.ListResult.Add(resultQuery);

                var bingResult = await bingService.Search(query.Description);
                resultQuery = new ResultQuery
                {
                    ProviderId = (int)ProviderEnum.Bing,
                    Amount = bingResult.Information.TotalResults
                };
                query.ListResult.Add(resultQuery);

                ListQuery.Add(query);

                var builder2 = new StringBuilder();
                builder2.Append(item + ": ");
                builder2.Append(googleService.ProviderName());
                builder2.Append(": ");
                builder2.Append(googleResult.Information.TotalResults);
                builder2.Append(" ");
                builder2.Append(bingService.ProviderName());
                builder2.Append(": ");
                builder2.Append(bingResult.Information.TotalResults);

                Console.WriteLine(builder2.ToString());
            }

            foreach (var item in ListProviders)
            {
                var builder3 = new StringBuilder();
                builder3.Append(item.Description);
                builder3.Append(" Winner: ");
                builder3.Append(Functions.GetMaxByProvider(ListQuery, item.Id));

                Console.WriteLine(builder3.ToString());
            }

            var builder1 = new StringBuilder();
            builder1.Append("Total winner: ");
            builder1.Append(Functions.SumByProvider(ListQuery));

            Console.WriteLine(builder1.ToString());
        }
        #endregion

        #region Config
        static ServiceProvider ServiceCollection(IConfiguration Configuration)
        {
            var services = new ServiceCollection();

            services.AddSingleton<IGoogleSearchService, GoogleSearchService>();
            services.AddSingleton<IBingSearchService, BingSearchService>();
            services.AddSingleton(Configuration);

            return services.BuildServiceProvider();
        }

        static class ConfigurationManager
        {
            public static IConfiguration AppSetting { get; }
            static ConfigurationManager()
            {
                AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
            }
        }
        #endregion
    }
}
