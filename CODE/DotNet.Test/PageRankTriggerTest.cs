using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using DotNet;
using Microsoft.AspNetCore.Mvc;
using PAGE_RANK_FUNCTION_APP;
using INFO_TRACK_DATA_MODELS;

namespace DotNet.Test
{
    [TestClass]
    public class PageRankTriggerTest : FunctionTestHelper.FunctionTest
    {
        [TestMethod]
        public async Task ValidateScraperQueryScore()
        {
            Dictionary<String, StringValues> queryDictionary = new Dictionary<String, StringValues>();
            var anonymousRequestObject = new { Keywords = "efiling integration", DomainUrl = "https://www.infotrack.com" };
            string requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(anonymousRequestObject, Newtonsoft.Json.Formatting.Indented);
                        
            IActionResult objectIActionResult = await (new Function1()).Run(req: HttpRequestSetup(queryDictionary, requestBody));
            JsonResult resultObject = (JsonResult)objectIActionResult;
            Assert.AreEqual(30, ((IScraperQuery)resultObject.Value).Score);
        }
    }
}
