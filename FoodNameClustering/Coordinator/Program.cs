using System;
using Akka.Actor;
using Akka.Configuration;

namespace Esha.Analysis.FoodClusteringAgents.TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(
                @"akka {  
                    stdout-loglevel = DEBUG
                    loglevel = DEBUG
                    log-config-on-start = on        
                    actor {                
                        debug {  
                            receive = on 
                            autoreceive = on
                            lifecycle = on
                            event-stream = on
                            unhandled = on
                        }
                    }
                ");
            var system = ActorSystem.Create("ClusterFoodNames", config);

            try
            {
                var coordinator = system.ActorOf<FoodNameClusteringCoordinator>("coord");
                var searchEngine = new AbstractSearchEngine(new BingSearchEngineImpl());

                var foodName = "barbecue sauce, smoky";
                var request = new FoodSearchRequestMessage(searchEngine.CreateQuery(foodName), foodName, TimeSpan.FromMinutes(2));
                coordinator.Tell(request);

                Console.ReadLine();
            }
            finally
            {
                system.Terminate();
            }

            Console.ReadLine();
        }
    }
}
