using INFO_TRACK_DATA_MODELS;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PAGE_RANK_FUNCTION_APP;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(PAGE_RANK_FUNCTION_APP.Startup))]
namespace PAGE_RANK_FUNCTION_APP
{    
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IScraperQuery, ScraperQuery>();
            builder.Services.AddScoped<ICore, Core>();
            builder.Services.AddLogging();
        }
    }
}
