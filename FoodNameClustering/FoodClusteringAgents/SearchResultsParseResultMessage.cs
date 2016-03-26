using System;
using System.Collections.Generic;

namespace Esha.Analysis.FoodClusteringAgents
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