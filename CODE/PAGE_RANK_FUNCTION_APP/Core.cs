using INFO_TRACK_DATA_MODELS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PAGE_RANK_FUNCTION_APP
{
    class Core : ICore
    {
        public Core()
        {

        }

        public ScraperQuery Compute(IScraperQuery objectOfScraperQuery)
        {
            try
            {
                string stringSearchEngine = Environment.GetEnvironmentVariable("SearchEngine") == null? "https://www.google.com.au/search?num=100&q={0}" : Environment.GetEnvironmentVariable("SearchEngine");
                string stringKeywords = string.Format(stringSearchEngine, HttpUtility.UrlEncode(objectOfScraperQuery.Keywords));
                Uri objectOfUri = new Uri(objectOfScraperQuery.DomainUrl);

                int intOccurrenceCount = 0;
                List<int> listOfMatchedPositions = new List<int>();

                HttpWebRequest objectOfHttpWebRequest = (HttpWebRequest)WebRequest.Create(stringKeywords);
                using (HttpWebResponse objectOfHttpWebResponse = (HttpWebResponse)objectOfHttpWebRequest.GetResponse())
                {
                    using (StreamReader objectOfStreamReader = new StreamReader(objectOfHttpWebResponse.GetResponseStream(), Encoding.ASCII))
                    {
                        string stringHtmlResponse = objectOfStreamReader.ReadToEnd();
                        string stringRegEx = Environment.GetEnvironmentVariable("RegExPattern") == null? "http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?" : Environment.GetEnvironmentVariable("RegExPattern");

                        MatchCollection objectOfMatchCollection = Regex.Matches(stringHtmlResponse, stringRegEx);                        
                        for (int intLoopCounter = 0; intLoopCounter < objectOfMatchCollection.Count; intLoopCounter++)
                        {
                            try
                            {
                                string match = objectOfMatchCollection[intLoopCounter].Groups[0].Value;
                                if (match.Contains(objectOfUri.Host))
                                {
                                    intOccurrenceCount++;
                                    listOfMatchedPositions.Add(intLoopCounter + 1);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
                objectOfScraperQuery.SearchResult = GetFormattedSearchResults(intOccurrenceCount, listOfMatchedPositions);
                objectOfScraperQuery.Score = GetScoreCount(listOfMatchedPositions);
            }
            catch (Exception objectException)
            {
                throw objectException;
            }
            return (ScraperQuery)objectOfScraperQuery;
        }

        private string GetFormattedSearchResults(int intOccurrenceCount, List<int> listOfMatchedPositions)
        {
            string stringReturnValue = string.Format($"No. of times your domain occurred in the search results: 0; Position(s): 0");
            try
            {
                StringBuilder objectOfStringBuilder = new StringBuilder();
                objectOfStringBuilder.Append($"No. of times your domain occurred in the search results: {intOccurrenceCount}; Positions: ");

                if (listOfMatchedPositions.Count == 0)
                {
                    objectOfStringBuilder.Append("0");
                }
                else
                {
                    int intCounter = 0;
                    foreach (int intCurrentPosition in listOfMatchedPositions)
                    {
                        if (intCounter > 0)
                        {
                            objectOfStringBuilder.Append($", {intCurrentPosition}");
                        }
                        else
                        {
                            intCounter = 1;
                            objectOfStringBuilder.Append($"{intCurrentPosition}");
                        }
                    }
                }
                stringReturnValue = objectOfStringBuilder.ToString();
            }
            catch (Exception)
            {

            }
            return stringReturnValue;
        }
    
        private int GetScoreCount(List<int> listOfMatchedPositions)
        {
            int intScoreCount = 0;
            try
            {
                foreach (int intPosition in listOfMatchedPositions)
                {
                    switch (intPosition)
                    {
                        case int n when (n <= 10):
                            intScoreCount += 10;
                            break;

                        case int n when (n > 10 && n <= 20):
                            intScoreCount += 8;
                            break;

                        case int n when (n > 20 && n <= 30):
                            intScoreCount += 6;
                            break;

                        case int n when (n > 30 && n <= 40):
                            intScoreCount += 4;
                            break;

                        case int n when (n > 40 && n <= 50):
                            intScoreCount += 2;
                            break;

                        case int n when (n > 50):
                            intScoreCount += 1;
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
            return intScoreCount;
        }
    }
}
