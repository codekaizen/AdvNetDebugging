using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class FoodSearchFailedMessage : FailureMessage<FoodSearchRequestMessage>
    {
        public FoodSearchFailedMessage(FoodSearchRequestMessage request, Exception exception = null)
            : base(request, exception)
        { }
    }
}