using System;
using Akka.Actor;

namespace Coordinator
{
    public class StoreMetricsActor : ReceiveActor
    {
        public StoreMetricsActor()
        {
            Receive<StoreMetricsRequestMessage>(async r =>
            {
                try
                {
                    Sender.Tell(new StoreMetricsResultMessage(r));
                }
                catch (Exception exp)
                {
                    Sender.Tell(new StoreMetricsFailedMessage(r, exp));
                }
            });
        }
    }
}