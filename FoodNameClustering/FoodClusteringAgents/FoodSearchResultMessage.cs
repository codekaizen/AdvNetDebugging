using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class FoodSearchResultMessage
    {
        public FoodSearchResultMessage(FoodSearchRequestMessage request, String htmlResult)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (htmlResult == null)
            {
                throw new ArgumentNullException(nameof(htmlResult));
            }
            Request = request;
            HtmlResult = htmlResult;
        }

        public FoodSearchRequestMessage Request { get; }
        public String HtmlResult { get; }
    }
}