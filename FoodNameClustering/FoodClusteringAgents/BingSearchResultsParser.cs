using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;

namespace Esha.Analysis.FoodClusteringAgents
{
    internal class BingSearchResultsParser : ISearchResultsParser
    {
        private readonly Func<Uri, CancellationToken, Task<IDocument>> _documentLoader;

        public BingSearchResultsParser(Func<Uri, CancellationToken, Task<IDocument>> documentLoader = null)
        {
            _documentLoader = documentLoader;
        }

        public async Task<IEnumerable<Uri>> ParseResultsAsync(SearchResults searchResults, CancellationToken cancellationToken)
        {
            var address = searchResults.SourceUri;
            var document = await (_documentLoader?.Invoke(address, cancellationToken) ?? defaultLoadDocument(address, cancellationToken));
            var anchors = document.QuerySelectorAll("li.b_algo a");
            return from a in anchors
                   let href = a.Attributes["href"]?.Value
                   where !String.IsNullOrWhiteSpace(href)
                   let uri = new Uri(href)
                   where String.Equals(uri.Scheme, "http", StringComparison.OrdinalIgnoreCase) ||
                         String.Equals(uri.Scheme, "https", StringComparison.OrdinalIgnoreCase)
                   select uri;
        }

        private async Task<IDocument> defaultLoadDocument(Uri address, CancellationToken cancellationToken)
        {
            var config = Configuration.Default.WithDefaultLoader();
            return await BrowsingContext.New(config).OpenAsync(new Url(address.ToString()), cancellationToken);
        }
    }
}