using System;
using System.Collections.Generic;
using System.Net.Http;
using Akka.Actor;

namespace Coordinator
{
    public class FoodNameClusteringCoordinator : ReceiveActor
    {
        private readonly Dictionary<String, IActorRef> _hostDownloader = new Dictionary<String, IActorRef>(StringComparer.InvariantCultureIgnoreCase);

        public FoodNameClusteringCoordinator()
        {
            Receive<FoodSearchRequestMessage>(r => handleHttpRequesetMessage(r));
            Receive<FoodSearchFailedMessage>(f => handleHttpRequestFailed(f));
        }

        private void handleHttpRequestFailed(FoodSearchFailedMessage failure)
        {
        }

        private void handleHttpRequesetMessage(FoodSearchRequestMessage message)
        {
            var host = message.SearchUri.Host;
            IActorRef child;

            if (!_hostDownloader.TryGetValue(host, out child))
            {
                child = Context
                    .ActorOf(Props.Create(() => new FoodNameSearchActor()));
                //.WithRouter(new RoundRobinPool(1, new DefaultResizer(0, 10))));
                _hostDownloader.Add(host, child);
            }

            child.Tell(message, Sender);
        }
    }
}