using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using INFO_TRACK_DATA_MODELS;

namespace PAGE_RANK_FUNCTION_APP
{
    public class Function1
    {

        private readonly IScraperQuery _ScraperQuery;
        private readonly ICore _Core;

        public Function1()
        {
            //Shortcut for supporting unit test cases - can be avoided.
            this._ScraperQuery = new ScraperQuery();
            this._Core = new Core();
        }

        /// <summary>
        /// Supports dependency injection.
        /// </summary>
        /// <param name="objectOfScraperQuery">Instance of IScraperQuery</param>
        public Function1(IScraperQuery objectOfScraperQuery, ICore objectOfCore)
        {
            this._ScraperQuery = objectOfScraperQuery;
            this._Core = objectOfCore;
        }

        [FunctionName("PageRank")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PageRankRequest objectOfPageRankRequest = JsonConvert.DeserializeObject<PageRankRequest>(requestBody);

            this._ScraperQuery.Keywords = objectOfPageRankRequest.Keywords;
            this._ScraperQuery.DomainUrl = objectOfPageRankRequest.DomainUrl;
            IScraperQuery objectScraperQuery = this._Core.Compute(this._ScraperQuery);

            return new JsonResult(new ScraperQuery() { Keywords = objectOfPageRankRequest.Keywords
                , DomainUrl = objectOfPageRankRequest.DomainUrl
                , Score = objectScraperQuery.Score
                , SearchResult = objectScraperQuery.SearchResult
                , RecordTimestamp = DateTime.UtcNow });
        }
    }
}
