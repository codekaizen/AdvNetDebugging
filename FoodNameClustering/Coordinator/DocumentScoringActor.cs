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
                    Sender.Tell(new ScoreDocumentResultMessage(r, score));
                }
                catch (Exception exp)
                {
                    Sender.Tell(new ScoreDocumentFailedMessage(r, exp));
                }
            });
        }

        private async Task<DocumentScore> scoreDocumentForTermsAsync(SearchResultDocument document, FoodNameTerms terms)
        {
            throw new NotImplementedException();
        }
    }
}