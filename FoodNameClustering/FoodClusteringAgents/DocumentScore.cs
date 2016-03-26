using System;
using System.Diagnostics;

namespace Esha.Analysis.FoodClusteringAgents
{
    [DebuggerDisplay("{FoodNameTerms} {ScoreVector.DebugView} in {Document.DocumentUri}")]
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