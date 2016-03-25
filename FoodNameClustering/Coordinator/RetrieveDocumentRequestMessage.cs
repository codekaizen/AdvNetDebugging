using System;

namespace Coordinator
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

        public Uri DocumentUri { get; set; }
        public TimeSpan Timeout { get; set; }
        public Uri SearchUri { get; set; }
        public String FoodNameQuery { get; set; }
    }
}