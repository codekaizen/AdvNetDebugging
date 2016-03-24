using System;

namespace Coordinator
{
    public class SearchResults
    {
        public SearchResults(Uri sourceUri, String foodName, String htmlContent)
        {
            SourceUri = sourceUri;
            FoodName = foodName;
            HtmlContent = htmlContent;
        }

        public Uri SourceUri { get; }
        public String FoodName { get; }
        public String HtmlContent { get; }
    }
}