using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionTestHelper
{
    public abstract class FunctionTest
    {
        public HttpRequest HttpRequestSetup(Dictionary<String, StringValues> query, string requestBody)
        {
            Mock<HttpRequest> objectMockHttpRequest = new Mock<HttpRequest>();
            objectMockHttpRequest.Setup(currentRequest => currentRequest.Query).Returns(new QueryCollection(query));
            MemoryStream objectMemoryStream = new MemoryStream();
            StreamWriter objectStreamWriter = new StreamWriter(objectMemoryStream);
            objectStreamWriter.Write(requestBody);
            objectStreamWriter.Flush();
            objectMemoryStream.Position = 0;
            objectMockHttpRequest.Setup(req => req.Body).Returns(objectMemoryStream);
            return objectMockHttpRequest.Object;
        }
    }
}
