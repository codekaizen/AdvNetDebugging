namespace Esha.Analysis.FoodClusteringAgents
{
    public class ScoreDocumentRequestMessage
    {
        public ScoreDocumentRequestMessage(SearchResultDocument document, FoodNameTerms foodNameTerms, FoodNameTerms sourceFoodTerms)
        {
            Document = document;
            FoodNameTerms = foodNameTerms;
            SourceFoodTerms = sourceFoodTerms;
        }

        public SearchResultDocument Document { get; }
        public FoodNameTerms FoodNameTerms { get; }
        public FoodNameTerms SourceFoodTerms { get; }
    }
}