using System;
using Akka.Actor;

namespace Coordinator
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("Cluster Food Names");

            try
            {
                var coordinator = system.ActorOf<FoodNameClusteringCoordinator>("coord");
                coordinator.Tell(new FoodSearchRequestMessage { SearchUri  = new Uri("http://www.bing.com") });
                Console.ReadLine();
            }
            finally
            {
                system.Terminate();
            }
        }
    }
}
