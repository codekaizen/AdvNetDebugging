using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class StoreMetricsFailedMessage : FailureMessage<StoreMetricsRequestMessage>
    {
        public StoreMetricsFailedMessage(StoreMetricsRequestMessage request, Exception exp)
            : base(request, exp) { }
    }
}