namespace Coordinator
{
    public class DocumentScore
    {
        public SearchResultDocument Document { get; set; }
        public FoodNameTerms FoodNameTerms { get; set; }
        public FoodNameScoreVector ScoreVector { get; set; }
    }
}