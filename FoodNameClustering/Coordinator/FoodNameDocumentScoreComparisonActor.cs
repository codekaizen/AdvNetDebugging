using System;
using System.Threading.Tasks;
using Akka.Actor;

namespace Coordinator
{
    public class FoodNameDocumentScoreComparisonActor : ReceiveActor
    {
        public FoodNameDocumentScoreComparisonActor()
        {
            Receive<CompareDocumentsRequestMessage>(async r =>
            {
                try
                {
                    var source = r.SourceScore;
                    var target = r.TargetScore;
                    var comparison = await compareScoresAsync(source, target);
                    Sender.Tell(new CompareDocumentsResultMessage(r, comparison));
                }
                catch (Exception exp)
                {
                    Sender.Tell(new CompareDocumentsFailedMessage(r, exp));
                }
            });
        }

        private async Task<ScoresDistance> compareScoresAsync(DocumentScore source, DocumentScore target)
        {
            throw new NotImplementedException();
        }
    }
}