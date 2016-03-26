using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;

namespace Coordinator
{
    public class FoodNameClusteringCoordinator : ReceiveActor
    {
        private readonly ConcurrentDictionary<Object, IActorRef> _agentCache = new ConcurrentDictionary<Object, IActorRef>();
        private readonly ConcurrentDictionary<Tuple<FoodNameTerms, Uri>, DocumentScore> _sourceScores = new ConcurrentDictionary<Tuple<FoodNameTerms, Uri>, DocumentScore>();
        private IReadOnlyCollection<String> _foodNames = new [] { "barbecue sauce, smoky", "barbecue sauce, mesquite", "chilli sauce", "ice cream, mint, chocolate, cookie" };

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

        private void handle(StoreMetricsFailedMessage message)
        {
        }

        private void handle(StoreMetricsResultMessage message)
        {
        }

        private void handle(StoreMetricsRequestMessage message)
        {
            var child = getChildActor(nameof(StoreMetricsActor), () => new StoreMetricsActor());
            child.Tell(message);
        }

        private void handle(CompareDocumentsFailedMessage message)
        {
        }

        private void handle(CompareDocumentsResultMessage message)
        {
            Self.Tell(new StoreMetricsRequestMessage(message.Comparison));
        }

        private void handle(CompareDocumentsRequestMessage message)
        {
            var child = getChildActor(nameof(FoodNameDocumentScoreComparisonActor), () => new FoodNameDocumentScoreComparisonActor());
            child.Tell(message);
        }

        private void handle(ScoreDocumentFailedMessage message)
        {
        }

        private void handle(ScoreDocumentResultMessage message)
        {
            DocumentScore sourceScore;

            var key = Tuple.Create(message.Request.SourceFoodTerms, message.Score.Document.DocumentUri);

            if (message.IsSourceScore)
            {
                _sourceScores.TryAdd(key, message.Score);
                return;
            }

            if (_sourceScores.TryGetValue(key, out sourceScore))
            {
                Self.Tell(new CompareDocumentsRequestMessage(sourceScore, message.Score));
            }
            else
            {
                // Retry later if the document isn't present
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    Self.Tell(message);
                });
            }
        }

        private void handle(ScoreDocumentRequestMessage message)
        {
            var key = Tuple.Create(nameof(DocumentScoringActor), message.Document.DocumentUri);
            var child = getChildActor(key, () => new DocumentScoringActor());
            child.Tell(message);
        }

        private void handle(RetrieveDocumentFailedMessage message)
        {
        }

        private void handle(RetrieveDocumentResultMessage message)
        {
            var foodNameQuery = message.Request.FoodNameQuery;
            var foodNameTerms = new FoodNameTerms(foodNameQuery);
            Self.Tell(new ScoreDocumentRequestMessage(message.SearchResultDoc, foodNameTerms, foodNameTerms));

            foreach (var foodName in _foodNames.Where(n => !n.Equals(foodNameQuery)))
            {
                Self.Tell(new ScoreDocumentRequestMessage(message.SearchResultDoc, new FoodNameTerms(foodName), foodNameTerms));
            }
        }

        private void handle(RetrieveDocumentRequestMessage message)
        {
            var key = Tuple.Create(nameof(DocumentRetrievalActor), message.DocumentUri);
            var child = getChildActor(key, () => new DocumentRetrievalActor());
            child.Tell(message);
        }

        private void handle(SearchResultsParseFailedMessage message)
        {
        }

        private void handle(SearchResultsParseResultMessage message)
        {
            var searchResults = message.Request.SearchResults;
            var searchUri = searchResults.SourceUri;
            var originatingFoodName = searchResults.FoodName;

            foreach (var doc in message.Documents)
            {
                Self.Tell(new RetrieveDocumentRequestMessage(doc, TimeSpan.FromMinutes(2), searchUri, originatingFoodName));
            }
        }

        private void handle(SearchResultsParseRequestMessage message)
        {
            var searchHost = message.SearchResults.SourceUri.Host;
            var key = Tuple.Create(nameof(SearchResultsParseActor), searchHost.ToLowerInvariant());
            var resultsParserImpl = createResultsParser(searchHost);
            var child = getChildActor(key, () => new SearchResultsParseActor(resultsParserImpl));
            child.Tell(message);
        }

        private void handle(FoodSearchResultMessage message)
        {
            var searchResults = new SearchResults(message.Request.SearchUri, message.Request.FoodName, message.HtmlResult);
            var request = new SearchResultsParseRequestMessage(searchResults);
            Self.Tell(request);
        }

        private void handle(FoodSearchFailedMessage failure)
        {
        }

        private void handle(FoodSearchRequestMessage message)
        {
            var key = Tuple.Create(nameof(FoodNameSearchActor), message.SearchUri.Host.ToLowerInvariant());
            var child = getChildActor(key, () => new FoodNameSearchActor());
            child.Tell(message);
        }

        private IActorRef getChildActor<TActor>(Object key, Expression<Func<TActor>> agentFactory)
            where TActor : ActorBase
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            IActorRef child;

            if (_agentCache.TryGetValue(key, out child))
            {
                return child;
            }

            child = Context.ActorOf(Props.Create(agentFactory).WithRouter(new RoundRobinPool(1, new DefaultResizer(1, 10))));
            _agentCache.TryAdd(key, child);

            return child;
        }

        private ISearchResultsParser createResultsParser(String searchEngineName)
        {
            searchEngineName = searchEngineName.ToLowerInvariant();

            switch (searchEngineName)
            {
                case "www.bing.com":
                case "bing.com":
                    return new BingSearchResultsParser();
                default:
                    throw new ArgumentOutOfRangeException("Unkown search engine host");
            }
        }
    }
}