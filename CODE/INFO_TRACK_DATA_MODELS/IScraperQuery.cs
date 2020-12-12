using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO_TRACK_DATA_MODELS
{
    public interface IScraperQuery
    {
        string Keywords { get; set; }

        string DomainUrl { get; set; }

        int Score { get; set; }

        string SearchResult { get; set; }

        DateTime RecordTimestamp { get; set; }
    }
}
