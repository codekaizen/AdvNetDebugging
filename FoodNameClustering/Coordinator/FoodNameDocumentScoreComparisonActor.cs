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

        private static async Task<ScoresDistance> compareScoresAsync(DocumentScore source, DocumentScore target)
        {
            return await Task.Run(() =>
            {
                Double sumSquares = 0;
                var sourceScores = source.ScoreVector;
                var targetScores = target.ScoreVector;

                for (var j = 0; j < sourceScores.Length; j++)
                {
                    var residual = sourceScores[j] - targetScores[j];
                    sumSquares += residual*residual;
                }

                return new ScoresDistance(source, target, Math.Sqrt(sumSquares));
            });
        }
    }
}