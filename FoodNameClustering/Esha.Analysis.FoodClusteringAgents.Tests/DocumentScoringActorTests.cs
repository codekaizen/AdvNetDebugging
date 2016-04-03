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

            var result = ExpectMsg<ScoreDocumentResultMessage>(duration: TimeSpan.FromMinutes(2));
            Assert.NotNull(result);
        }
    }
}