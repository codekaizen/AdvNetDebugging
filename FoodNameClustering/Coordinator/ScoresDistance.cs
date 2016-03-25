using System;

namespace Coordinator
{
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