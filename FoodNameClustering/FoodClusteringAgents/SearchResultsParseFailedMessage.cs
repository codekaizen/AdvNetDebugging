using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class SearchResultsParseFailedMessage : FailureMessage<SearchResultsParseRequestMessage>
    {
        public SearchResultsParseFailedMessage(SearchResultsParseRequestMessage request, Exception exp)
            : base(request, exp)
        { }
    }
}