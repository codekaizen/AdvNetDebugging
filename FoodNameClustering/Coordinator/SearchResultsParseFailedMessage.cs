using System;

namespace Coordinator
{
    public class SearchResultsParseFailedMessage : FailureMessage<SearchResultsParseRequestMessage>
    {
        public SearchResultsParseFailedMessage(SearchResultsParseRequestMessage request, Exception exp)
            : base(request, exp)
        { }
    }
}