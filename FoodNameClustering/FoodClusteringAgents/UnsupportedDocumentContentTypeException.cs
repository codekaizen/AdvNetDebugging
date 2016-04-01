using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class UnsupportedDocumentContentTypeException : Exception
    {
        public UnsupportedDocumentContentTypeException(String contentType, Uri documentUri)
        {
            ContentType = contentType;
            DocumentUri = documentUri;
        }

        public String ContentType { get; set; }
        public Uri DocumentUri { get; set; }
    }
}