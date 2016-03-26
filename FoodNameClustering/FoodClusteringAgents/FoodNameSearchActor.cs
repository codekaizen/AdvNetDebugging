using System;
using System.Net.Http;
using Akka.Actor;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class FoodNameSearchActor : HttpClientReceiveActor
    {
        public FoodNameSearchActor()
        {
            Receive<FoodSearchRequestMessage>(async r =>
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, r.SearchUri);
                    var response = await DoHttpRequestAsync(request, r.TimeOut);
                    var htmlResult = await response.Content.ReadAsStringAsync();
                    Sender.Tell(new FoodSearchResultMessage(r, htmlResult));
                }
                catch (Exception exp)
                {
                    Sender.Tell(new FoodSearchFailedMessage(r, exp));
                }
            });
        }
    }
}