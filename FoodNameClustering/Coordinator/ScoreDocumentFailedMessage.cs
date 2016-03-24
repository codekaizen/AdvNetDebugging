using System;

namespace Coordinator
{
    public class ScoreDocumentFailedMessage : FailureMessage<ScoreDocumentRequestMessage>
    {
        public ScoreDocumentFailedMessage(ScoreDocumentRequestMessage request, Exception exp)
            : base(request, exp) { }
    }
}