using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esha.Analysis.FoodClusteringAgents
{
    public interface ISearchResultsParser
    {
        Task<IEnumerable<Uri>> ParseResultsAsync(SearchResults searchResults);
    }
}