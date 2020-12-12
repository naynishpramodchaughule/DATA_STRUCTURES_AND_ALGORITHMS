using INFO_TRACK_DATA_MODELS;
using INFO_TRACK_WEB_SCRAPER.Interfaces;
using INFO_TRACK_WEB_SCRAPER.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace INFO_TRACK_WEB_SCRAPER.Controllers
{
    public class WebScraperController : InfoTrackBaseController
    {
        private readonly IWebScraperViewModel _IWebScraperViewModel;

        /// <summary>
        /// Uses Microsoft Unity IOC container for dependency injection.
        /// </summary>
        /// <param name="instanceOfIWebScraperViewModel">An instance of IWebScraperViewModel interface.</param>
        public WebScraperController(IWebScraperViewModel instanceOfIWebScraperViewModel)
        {
            _IWebScraperViewModel = instanceOfIWebScraperViewModel;            
        }

        public override ActionResult Index()
        {
            try
            {   
                //TempData stores the data temporarily and automatically removes it after retrieving a value.
                TempData["ScraperQueryResult"] = _IWebScraperViewModel.ListOfScraperQueries;
                return View("Statistics", _IWebScraperViewModel);
            }
            catch (Exception objectException)
            {
                throw new Exception(message: objectException.Message, innerException: objectException.InnerException);
            }
        }

        [HttpPost]
        [ActionName("Statistics")]
        public async Task<ActionResult> ComputeStatistics(WebScraperViewModel objectWebScraperViewModel)
        {
            try
            {
                List<ScraperQuery> objectListOfScraperQueryResult = null;
                if (TempData["ScraperQueryResult"] != null)
                {
                    objectListOfScraperQueryResult = TempData["ScraperQueryResult"] as List<ScraperQuery>;
                }
                //Keep it for the subsequent request.
                TempData.Keep();

                if (ModelState.IsValid)
                {
                    ScraperQuery objectScraperQuery = await GetSearchResults(objectWebScraperViewModel.Keywords, objectWebScraperViewModel.DomainUrl);

                    objectListOfScraperQueryResult.Add(new ScraperQuery()
                    {
                        Keywords = objectWebScraperViewModel.Keywords,
                        DomainUrl = objectWebScraperViewModel.DomainUrl,
                        Score = objectScraperQuery.Score,
                        SearchResult = objectScraperQuery.SearchResult,                        
                        RecordTimestamp = DateTime.Now.ToUniversalTime()
                    });

                    TempData["ScraperQueryResult"] = objectListOfScraperQueryResult;
                    objectWebScraperViewModel.ListOfScraperQueries = objectListOfScraperQueryResult;                    
                    return View("Statistics", objectWebScraperViewModel);
                }

                //If the ModelState is not valid.
                objectWebScraperViewModel.ListOfScraperQueries = objectListOfScraperQueryResult;                
                return View("Statistics", objectWebScraperViewModel);
            }
            catch (Exception objectException)
            {
                throw new Exception(message: objectException.Message, innerException: objectException.InnerException);
            }
        }

        /// <summary>
        /// Calls Azure function app to get the search results and score.
        /// </summary>
        /// <param name="stringKeywords"></param>
        /// <param name="stringDomainUrl"></param>
        /// <returns></returns>
        private async Task<ScraperQuery> GetSearchResults(string stringKeywords, string stringDomainUrl)
        {
            ScraperQuery objectScraperQuery = new ScraperQuery() { Keywords = stringKeywords, DomainUrl = stringDomainUrl };
            try
            {
                HttpClient objectHttpClient = new HttpClient();
                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(new { Keywords = stringKeywords, DomainUrl = stringDomainUrl }), Encoding.UTF8, "application/json");
                jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage objectHttpResponseMessage = await objectHttpClient.PostAsync(ConfigurationManager.AppSettings["PageRankHttpEndPoint"], jsonContent);
                if (objectHttpResponseMessage != null)
                {
                    string jsonResponseString = await objectHttpResponseMessage.Content.ReadAsStringAsync();
                    objectScraperQuery = JsonConvert.DeserializeObject<ScraperQuery>(jsonResponseString);
                }
            }
            catch (Exception objectException)
            {
                throw new Exception(message: objectException.Message, innerException: objectException.InnerException);
            }
            return objectScraperQuery;
        }
    }
}
