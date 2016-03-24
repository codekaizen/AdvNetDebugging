using System;
using System.Collections.Generic;

namespace Coordinator
{
    public class SearchResultsParseResultMessage : ResultMessage<SearchResultsParseRequestMessage>
    {
        public SearchResultsParseResultMessage(SearchResultsParseRequestMessage request, IEnumerable<Uri> documents)
            : base(request)
        {
            Documents = documents;
        }

        public IEnumerable<Uri> Documents { get; }
    }
}