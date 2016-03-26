using System;
using System.Diagnostics;

namespace Coordinator
{
    [DebuggerDisplay("{SourceScore.FoodNameTerms.ToString()} : {TargetScore.FoodNameTerms.ToString()} in {SourceScore.Document.DocumentUri} = {DifferenceNorm}")]
    public class ScoresDistance
    {
        public ScoresDistance(DocumentScore source, DocumentScore target, Double differenceNorm)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            SourceScore = source;
            TargetScore = target;
            DifferenceNorm = differenceNorm;
        }

        public DocumentScore SourceScore { get; }
        public DocumentScore TargetScore { get; }
        public Double DifferenceNorm { get; }
    }
}