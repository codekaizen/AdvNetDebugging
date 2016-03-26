using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;

namespace Esha.Analysis.FoodClusteringAgents
{
    internal class BingSearchResultsParser : ISearchResultsParser
    {
        public async Task<IEnumerable<Uri>> ParseResultsAsync(SearchResults searchResults)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var address = "http://www.bing.com/search?q=barbecue+sauce%2C+smoky";
            var document = await BrowsingContext.New(config).OpenAsync(address);
            var anchors = document.QuerySelectorAll("li.b_algo a");
            return from a in anchors
                let href = a.Attributes["href"]?.Value
                where !String.IsNullOrWhiteSpace(href)
                let uri = new Uri(href)
                where String.Equals(uri.Scheme, "http", StringComparison.OrdinalIgnoreCase) ||
                      String.Equals(uri.Scheme, "https", StringComparison.OrdinalIgnoreCase)
                select uri;
        }
    }
}