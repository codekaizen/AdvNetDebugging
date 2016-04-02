using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace Esha.Analysis.FoodClusteringAgents.Tests
{
    public class DocumentScoringActorTests : TestKit
    {
        [Fact]
        public void FactMethodName()
        {
            var sut = Sys.ActorOf(Props.Create(() => new DocumentScoringActor()));
            var searchUri = new Uri("http://google.com");
            var foodTerms = new FoodNameTerms("food stuff, mucho");
            var documentUri = new Uri("http://example.com");
            var documentVector = new DocumentVector(new[] { "foo", "bar" });
            var document = new SearchResultDocument(searchUri, foodTerms.FoodName, documentUri, documentVector);
            var compareTerms = foodTerms;
            var originatingTerms = foodTerms;
            sut.Tell(new ScoreDocumentRequestMessage(document, compareTerms, originatingTerms));
        }
    }

    public class DocumentRetrievalActorTests : TestKit
    {
        [Fact]
        public void UnacceptedMimeTypeRequestFails()
        {
            var sut = Sys.ActorOf(Props.Create(() => new DocumentRetrievalActor()));
            var documentUri = new Uri("http://i.imgur.com/XQ24eik.gif");
            var searchUri = new Uri("http://google.com");
            var foodName = "food stuff";
            sut.Tell(new RetrieveDocumentRequestMessage(documentUri, TimeSpan.FromMinutes(2), searchUri, foodName));
            var result = ExpectMsg<RetrieveDocumentFailedMessage>(duration: TimeSpan.FromMinutes(2));
            Assert.NotNull(result);
            Assert.IsType<UnsupportedDocumentContentTypeException>(result.Exception);
        }

        [Fact]
        public void SendsRetrieveDocumentResultMessage()
        {
            var sut = Sys.ActorOf(Props.Create(() => new DocumentRetrievalActor()));
            var documentUri = new Uri("http://example.com");
            var searchUri = new Uri("http://google.com");
            var foodName = "food stuff";
            sut.Tell(new RetrieveDocumentRequestMessage(documentUri, TimeSpan.FromMinutes(2), searchUri, foodName));
            var result = ExpectMsg<RetrieveDocumentResultMessage>();
            Assert.Equal(documentUri, result.SearchResultDoc.DocumentUri);
            Assert.Equal(searchUri, result.SearchResultDoc.SearchUri);
            Assert.Equal(foodName, result.SearchResultDoc.OriginatingFoodName);
            Assert.NotEmpty(result.SearchResultDoc.DocumentVector);
        }
    }
}