using System;
using Akka.Actor;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class SearchResultsParseActor : ReceiveActor
    {
        private readonly ISearchResultsParser _parserImpl;
        public SearchResultsParseActor(ISearchResultsParser parserImpl)
        {
            _parserImpl = parserImpl;

            Receive<SearchResultsParseRequestMessage>(async r =>
            {
                try
                {
                    var documents = await _parserImpl.ParseResultsAsync(r.SearchResults);
                    Sender.Tell(new SearchResultsParseResultMessage(r, documents));
                }
                catch (Exception exp)
                {
                    Sender.Tell(new SearchResultsParseFailedMessage(r, exp));
                }
            });
        }
    }
}