using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Xunit;

namespace Esha.Analysis.FoodClusteringAgents.Tests
{
    public class BingSearchResultsParserTests
    {
        [Fact]
        public async Task ParsingSearchDocumentReturnsUrisOfDocuments()
        {   
            IDocument document;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetType(), "BingSearchResults.html"))
            {
                document = await new HtmlParser().ParseAsync(stream);
            }

            var sut = new BingSearchResultsParser((uri, cancellationToken) => Task.FromResult(document));
            var results = await sut.ParseResultsAsync(new SearchResults(new Uri("http://bing.com/search/results"), "food stuff", null), CancellationToken.None);
            Assert.NotNull(results);
            Assert.Contains(results, uri => uri == new Uri("http://www.theyummylife.com/bbq_sauce_recipes_sweet_tangy_spicy_smoky"));
        }
    }
}
