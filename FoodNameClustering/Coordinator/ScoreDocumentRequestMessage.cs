namespace Coordinator
{
    public class ScoreDocumentRequestMessage
    {
        public ScoreDocumentRequestMessage(SearchResultDocument document, FoodNameTerms foodNameTerms)
        {
            Document = document;
            FoodNameTerms = foodNameTerms;
        }

        public SearchResultDocument Document { get; }
        public FoodNameTerms FoodNameTerms { get; }
    }
}