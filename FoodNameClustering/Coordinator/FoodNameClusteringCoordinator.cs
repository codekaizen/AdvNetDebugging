using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;

namespace Coordinator
{
    public class FoodNameClusteringCoordinator : ReceiveActor
    {
        private readonly ConcurrentDictionary<Object, IActorRef> _agentCache = new ConcurrentDictionary<Object, IActorRef>();
        private readonly ConcurrentDictionary<FoodNameTerms, DocumentScore> _sourceScores = new ConcurrentDictionary<FoodNameTerms, DocumentScore>();
        private IReadOnlyCollection<String> _foodNames;

        public FoodNameClusteringCoordinator()
        {
            Receive<FoodSearchRequestMessage>(r => handle(r));
            Receive<FoodSearchResultMessage>(r => handle(r));
            Receive<FoodSearchFailedMessage>(f => handle(f));

            Receive<SearchResultsParseRequestMessage>(m => handle(m));
            Receive<SearchResultsParseResultMessage>(m => handle(m));
            Receive<SearchResultsParseFailedMessage>(m => handle(m));

            Receive<RetrieveDocumentRequestMessage>(m => handle(m));
            Receive<RetrieveDocumentResultMessage>(m => handle(m));
            Receive<RetrieveDocumentFailedMessage>(m => handle(m));

            Receive<ScoreDocumentRequestMessage>(m => handle(m));
            Receive<ScoreDocumentResultMessage>(m => handle(m));
            Receive<ScoreDocumentFailedMessage>(m => handle(m));

            Receive<CompareDocumentsRequestMessage>(m => handle(m));
            Receive<CompareDocumentsResultMessage>(m => handle(m));
            Receive<CompareDocumentsFailedMessage>(m => handle(m));

            Receive<StoreMetricsRequestMessage>(m => handle(m));
            Receive<StoreMetricsResultMessage>(m => handle(m));
            Receive<StoreMetricsFailedMessage>(m => handle(m));
        }

        private Task handle(StoreMetricsFailedMessage message)
        {
            throw new NotImplementedException();
        }

        private Task handle(StoreMetricsResultMessage message)
        {
            throw new NotImplementedException();
        }

        private Task handle(StoreMetricsRequestMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(CompareDocumentsFailedMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(CompareDocumentsResultMessage message)
        {
            Sender.Tell(new StoreMetricsRequestMessage(message.Comparison));
        }

        private void handle(CompareDocumentsRequestMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(ScoreDocumentFailedMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(ScoreDocumentResultMessage message)
        {
            DocumentScore sourceScore;

            if (_sourceScores.TryGetValue(message.Request.SourceFoodTerms, out sourceScore))
            {
                Sender.Tell(new CompareDocumentsRequestMessage(sourceScore, message.Score));
            }
            else
            {
                // Retry later
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    Sender.Tell(message);
                });
            }
        }

        private void handle(ScoreDocumentRequestMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(RetrieveDocumentFailedMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(RetrieveDocumentResultMessage message)
        {
            var foodNameQuery = message.Request.FoodNameQuery;
            var foodNameTerms = new FoodNameTerms(foodNameQuery);
            Sender.Tell(new ScoreDocumentRequestMessage(message.SearchResultDoc, foodNameTerms, foodNameTerms));

            foreach (var foodName in _foodNames.Where(n => !n.Equals(foodNameQuery)))
            {
                Sender.Tell(new ScoreDocumentRequestMessage(message.SearchResultDoc, new FoodNameTerms(foodName), foodNameTerms));
            }
        }

        private void handle(RetrieveDocumentRequestMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(SearchResultsParseFailedMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(SearchResultsParseResultMessage message)
        {
            var searchResults = message.Request.SearchResults;
            var searchUri = searchResults.SourceUri;
            var originatingFoodName = searchResults.FoodName;

            foreach (var doc in message.Documents)
            {
                Sender.Tell(new RetrieveDocumentRequestMessage(doc, TimeSpan.FromMinutes(2), searchUri, originatingFoodName));
            }
        }

        private void handle(SearchResultsParseRequestMessage message)
        {
            throw new NotImplementedException();
        }

        private void handle(FoodSearchResultMessage message)
        {
            Sender.Tell(new SearchResultsParseRequestMessage
            {
                SearchResults = new SearchResults(message.Request.SearchUri, message.Request.FoodName, message.HtmlResult)
            });
        }

        private void handle(FoodSearchFailedMessage failure)
        {
        }

        private void handle(FoodSearchRequestMessage message)
        {
            var host = message.SearchUri.Host.ToLowerInvariant();
            IActorRef child;

            if (!_agentCache.TryGetValue(host, out child))
            {
                child = Context.ActorOf(Props.Create(() => new FoodNameSearchActor()));
                //.WithRouter(new RoundRobinPool(1, new DefaultResizer(0, 10))));
                _agentCache.TryAdd(host, child);
            }

            child.Tell(message, Sender);
        }
    }
}