using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class ScoreDocumentRequestMessage
    {
        public ScoreDocumentRequestMessage(SearchResultDocument document, FoodNameTerms foodNameTerms, FoodNameTerms sourceFoodTerms)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (foodNameTerms == null)
            {
                throw new ArgumentNullException(nameof(foodNameTerms));
            }

            if (sourceFoodTerms == null)
            {
                throw new ArgumentNullException(nameof(sourceFoodTerms));
            }

            Document = document;
            FoodNameTerms = foodNameTerms;
            SourceFoodTerms = sourceFoodTerms;
        }

        public String SearchUriValue => Document.SearchUri?.ToString();
        public SearchResultDocument Document { get; }
        public FoodNameTerms FoodNameTerms { get; }
        public FoodNameTerms SourceFoodTerms { get; }
    }
}