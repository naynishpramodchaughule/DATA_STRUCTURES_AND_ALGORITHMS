using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFO_TRACK_DATA_MODELS;

namespace INFO_TRACK_WEB_SCRAPER.Interfaces
{
    public interface IWebScraperViewModel
    {   
        string Keywords { get; set; }
        
        string DomainUrl { get; set; }

        List<ScraperQuery> ListOfScraperQueries { get; set; }
    }
}
