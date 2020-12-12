using INFO_TRACK_WEB_SCRAPER.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using INFO_TRACK_DATA_MODELS;

namespace INFO_TRACK_WEB_SCRAPER.ViewModels
{
    /// <summary>
    /// The view model is an abstraction of the view 'Statistics' exposing public properties and commands.
    /// </summary>
    public class WebScraperViewModel : IWebScraperViewModel
    {
        [Required(ErrorMessage = "Keywords are required")]
        [StringLength(maximumLength: 4000, MinimumLength = 3, ErrorMessage = "Min. 3 char(s) are required")]
        public string Keywords { get; set; }

        [Required(ErrorMessage = "Domain url is required")]
        [Url(ErrorMessage = "Domain url is invalid")]
        [StringLength(maximumLength: 4000, MinimumLength = 5, ErrorMessage = "Min. (5) - Max. (4000) length")]
        public string DomainUrl { get; set; }


        private List<ScraperQuery> _ListOfScraperQueries;
        public List<ScraperQuery> ListOfScraperQueries 
        { 
            get
            {
                if (this._ListOfScraperQueries is null || _ListOfScraperQueries.Count == 0)
                {
                    this._ListOfScraperQueries = GetDefaultScraperQueries();
                }
                return this._ListOfScraperQueries;
            }

            set 
            {   
                if (value != null)
                {
                    foreach (ScraperQuery objectCurrentScraperQuery in value)
                    {
                        if (!string.IsNullOrEmpty(objectCurrentScraperQuery.Keywords) && !string.IsNullOrEmpty(objectCurrentScraperQuery.DomainUrl))
                        {
                            this._ListOfScraperQueries.Add(objectCurrentScraperQuery);
                        }
                    }
                }

                if (this._ListOfScraperQueries is null || this._ListOfScraperQueries.Count == 0)
                {
                    this._ListOfScraperQueries = GetDefaultScraperQueries();
                }
            }
        }

        public string Message { get; set; }

        public WebScraperViewModel()
        {
            this._ListOfScraperQueries = new List<ScraperQuery>();
        }

        /// <summary>
        /// Sets defaults for the historic search records.
        /// </summary>
        /// <returns>A list of 'ScraperQuery' data model.</returns>
        private List<ScraperQuery> GetDefaultScraperQueries()
        {
            List<ScraperQuery> objectListOfDefaultScraperQueries = new List<ScraperQuery>();
            try
            {
                objectListOfDefaultScraperQueries.Add(new ScraperQuery() { Keywords = "efiling integration (Sample record)", DomainUrl = "https://www.infotrack.com", Score = 10, SearchResult = "Sample search results", RecordTimestamp = DateTime.Now.ToUniversalTime().AddDays(-180) });
                objectListOfDefaultScraperQueries.Add(new ScraperQuery() { Keywords = "test (Sample record)", DomainUrl = "https://www.facebook.com", Score = 5, SearchResult = "Sample search results", RecordTimestamp = DateTime.Now.ToUniversalTime().AddDays(-90) });
            }
            catch (Exception)
            {

            }
            return objectListOfDefaultScraperQueries;
        }
    }
}