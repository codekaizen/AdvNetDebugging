using System;

namespace Coordinator
{
    public class FoodSearchResultMessage
    {
        public FoodSearchResultMessage(FoodSearchRequestMessage request, String htmlResult)
        {
            Request = request;
            HtmlResult = htmlResult;
        }

        public FoodSearchRequestMessage Request { get; }
        public String HtmlResult { get; }
    }
}