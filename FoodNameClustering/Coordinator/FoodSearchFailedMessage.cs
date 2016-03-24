using System;

namespace Coordinator
{
    public class FoodSearchFailedMessage : FailureMessage<FoodSearchRequestMessage>
    {
        public FoodSearchFailedMessage(FoodSearchRequestMessage request, Exception exception = null)
            : base(request, exception)
        { }
    }
}