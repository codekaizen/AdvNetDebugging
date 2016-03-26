using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class ScoreDocumentResultMessage : ResultMessage<ScoreDocumentRequestMessage>
    {
        public ScoreDocumentResultMessage(ScoreDocumentRequestMessage request, DocumentScore score, Boolean isSourceScore)
            : base(request)
        {
            Score = score;
            IsSourceScore = isSourceScore;
        }

        public DocumentScore Score { get; }
        public bool IsSourceScore { get; }
    }
}