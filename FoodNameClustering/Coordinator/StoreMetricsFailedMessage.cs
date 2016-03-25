using System;

namespace Coordinator
{
    public class StoreMetricsFailedMessage : FailureMessage<StoreMetricsRequestMessage>
    {
        public StoreMetricsFailedMessage(StoreMetricsRequestMessage request, Exception exp)
            : base(request, exp) { }
    }
}