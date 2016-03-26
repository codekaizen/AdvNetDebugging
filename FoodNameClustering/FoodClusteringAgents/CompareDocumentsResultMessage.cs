namespace Esha.Analysis.FoodClusteringAgents
{
    public class CompareDocumentsResultMessage : ResultMessage<CompareDocumentsRequestMessage>
    {
        public CompareDocumentsResultMessage(CompareDocumentsRequestMessage request, ScoresDistance comparison)
            : base(request)
        {
            Comparison = comparison;
        }

        public ScoresDistance Comparison { get; }
    }
}