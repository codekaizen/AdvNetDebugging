using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class ScoreDocumentFailedMessage : FailureMessage<ScoreDocumentRequestMessage>
    {
        public ScoreDocumentFailedMessage(ScoreDocumentRequestMessage request, Exception exp)
            : base(request, exp) { }
    }
}