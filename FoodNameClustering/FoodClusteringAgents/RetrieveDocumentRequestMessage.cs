using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class RetrieveDocumentRequestMessage
    {
        public RetrieveDocumentRequestMessage(Uri documentUri, TimeSpan timeout, Uri searchUri, String foodNameQuery)
        {
            DocumentUri = documentUri;
            Timeout = timeout;
            SearchUri = searchUri;
            FoodNameQuery = foodNameQuery;
        }

        public Uri DocumentUri { get; }
        public TimeSpan Timeout { get; }
        public Uri SearchUri { get; }
        public String FoodNameQuery { get; }
    }
}