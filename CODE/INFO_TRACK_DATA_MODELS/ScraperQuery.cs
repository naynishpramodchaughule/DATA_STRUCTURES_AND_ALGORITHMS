using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO_TRACK_DATA_MODELS
{
    public class ScraperQuery: IScraperQuery
    {
        public string Keywords { get; set; }

        public string DomainUrl { get; set; }

        public int Score { get; set; }

        public string SearchResult { get; set; }

        public DateTime RecordTimestamp { get; set; }

        public ScraperQuery()
        {
            this.Keywords = "";
            this.DomainUrl = "";
            this.Score = 0;
            this.SearchResult = "";
            this.RecordTimestamp = DateTime.UtcNow;
        }
    }
}
