using System;

namespace Coordinator
{
    public class CompareDocumentsFailedMessage : FailureMessage<CompareDocumentsRequestMessage>
    {
        public CompareDocumentsFailedMessage(CompareDocumentsRequestMessage request, Exception exp)
            : base(request, exp)
        { }
    }
}