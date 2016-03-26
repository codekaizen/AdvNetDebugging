using System;
using System.Diagnostics;

namespace Esha.Analysis.FoodClusteringAgents
{
    [DebuggerDisplay("{SearchUri.Host} query of {OriginatingFoodName} result {DocumentUri}: {DocumentVector.ToAbbreviatedString()}")]
    public class SearchResultDocument
    {
        public SearchResultDocument(Uri searchUri, String originatingFoodName, Uri documentUri, DocumentVector documentVector)
        {
            SearchUri = searchUri;
            OriginatingFoodName = originatingFoodName;
            DocumentUri = documentUri;
            DocumentVector = documentVector;
        }

        public Uri SearchUri { get; }
        public String OriginatingFoodName { get; }
        public Uri DocumentUri { get; }
        public DocumentVector DocumentVector { get; }
    }
}