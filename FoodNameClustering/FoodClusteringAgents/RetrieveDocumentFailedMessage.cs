using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class RetrieveDocumentFailedMessage : FailureMessage<RetrieveDocumentRequestMessage>
    {
        public RetrieveDocumentFailedMessage(RetrieveDocumentRequestMessage request, Exception exp)
            : base(request, exp)
        { }
    }
}