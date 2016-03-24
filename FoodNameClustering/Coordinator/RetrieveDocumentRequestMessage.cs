using System;

namespace Coordinator
{
    public class RetrieveDocumentRequestMessage
    {
        public RetrieveDocumentRequestMessage(Uri documentUri, TimeSpan timeout, Uri searchUri, String originatingFoodName)
        {
            DocumentUri = documentUri;
            Timeout = timeout;
            SearchUri = searchUri;
            OriginatingFoodName = originatingFoodName;
        }

        public Uri DocumentUri { get; }
        public TimeSpan Timeout { get; }
        public Uri SearchUri { get; }
        public String OriginatingFoodName { get; }
    }
}