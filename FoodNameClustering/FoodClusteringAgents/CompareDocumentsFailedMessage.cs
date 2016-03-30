using System;
using System.Diagnostics;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class CompareDocumentsFailedMessage : FailureMessage<CompareDocumentsRequestMessage>
    {
        public CompareDocumentsFailedMessage(CompareDocumentsRequestMessage request, Exception exp)
            : base(request, exp)
        { }
    }
}