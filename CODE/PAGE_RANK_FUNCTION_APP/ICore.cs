using INFO_TRACK_DATA_MODELS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAGE_RANK_FUNCTION_APP
{
    public interface ICore
    {
        ScraperQuery Compute(IScraperQuery objectOfScraperQuery);
    }
}
