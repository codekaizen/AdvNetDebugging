using System;

namespace Coordinator
{
    public class RetrieveDocumentFailedMessage : FailureMessage<RetrieveDocumentRequestMessage>
    {
        public RetrieveDocumentFailedMessage(RetrieveDocumentRequestMessage request, Exception exp)
            : base(request, exp)
        { }
    }
}