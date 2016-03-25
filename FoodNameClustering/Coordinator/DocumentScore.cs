using System;

namespace Coordinator
{
    public class DocumentScore
    {
        public DocumentScore(SearchResultDocument document, FoodNameTerms terms, FoodNameScoreVector foodNameScoreVector)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            if (terms == null)
            {
                throw new ArgumentNullException(nameof(terms));
            }
            if (foodNameScoreVector == null)
            {
                throw new ArgumentNullException(nameof(foodNameScoreVector));
            }

            Document = document;
            FoodNameTerms = terms;
            ScoreVector = foodNameScoreVector;
        }

        public SearchResultDocument Document { get; }
        public FoodNameTerms FoodNameTerms { get; }
        public FoodNameScoreVector ScoreVector { get; }
    }
}