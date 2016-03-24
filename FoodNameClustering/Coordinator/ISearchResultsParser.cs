using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coordinator
{
    public interface ISearchResultsParser
    {
        Task<IEnumerable<Uri>> ParseResultsAsync(SearchResults searchResults);
    }
}