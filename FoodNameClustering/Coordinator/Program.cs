using System;
using Akka.Actor;

namespace Coordinator
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("ClusterFoodNames");

            try
            {
                var coordinator = system.ActorOf<FoodNameClusteringCoordinator>("coord");
                var searchEngine = new AbstractSearchEngine(new BingSearchEngineImpl());

                var foodName = "barbecue sauce, smokey";
                coordinator.Tell(new FoodSearchRequestMessage
                {
                    SearchUri = searchEngine.CreateQuery(foodName),
                    FoodName = foodName,
                    TimeOut = TimeSpan.FromMinutes(2)
                });
                Console.ReadLine();
            }
            finally
            {
                system.Terminate();
            }
        }
    }
}
