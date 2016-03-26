namespace Esha.Analysis.FoodClusteringAgents
{
    public class StoreMetricsRequestMessage
    {
        public StoreMetricsRequestMessage(ScoresDistance comparison)
        {
            Comparison = comparison;
        }

        public ScoresDistance Comparison { get; }
    }
}