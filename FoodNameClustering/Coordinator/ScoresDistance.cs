using System;

namespace Coordinator
{
    public class ScoresDistance
    {
        public DocumentScore SourceScore { get; set; }
        public DocumentScore TargetScore { get; set; }
        public Double DifferenceNorm { get; set; }
    }
}