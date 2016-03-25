using System;
using System.Threading.Tasks;
using Akka.Actor;

namespace Coordinator
{
    public class DocumentScoringActor : ReceiveActor
    {
        public DocumentScoringActor()
        {
            Receive<ScoreDocumentRequestMessage>(async r =>
            {
                try
                {
                    var document = r.Document;
                    var terms = r.FoodNameTerms;
                    var score = await scoreDocumentForTermsAsync(document, terms);
                    Sender.Tell(new ScoreDocumentResultMessage(r, score, r.FoodNameTerms.Equals(r.SourceFoodTerms)));
                }
                catch (Exception exp)
                {
                    Sender.Tell(new ScoreDocumentFailedMessage(r, exp));
                }
            });
        }

        private async Task<DocumentScore> scoreDocumentForTermsAsync(SearchResultDocument document, FoodNameTerms terms)
        {
            return await Task.Run(() =>
            {
                var documentVector = document.DocumentVector;
                var score = new Double[documentVector.Length];

                for (var i = 0; i < documentVector.Length; i++)
                {
                    if (terms.Contains(documentVector[i]))
                    {
                        score[i] = 1.0;
                    }
                }

                return new DocumentScore(document, terms, new FoodNameScoreVector(score));
            });
        }
    }
}