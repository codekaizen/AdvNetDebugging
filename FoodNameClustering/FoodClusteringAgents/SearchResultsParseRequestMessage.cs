namespace Esha.Analysis.FoodClusteringAgents
{
    public class SearchResultsParseRequestMessage
    {
        public SearchResultsParseRequestMessage(SearchResults searchResults)
        {
            SearchResults = searchResults;
        }

        public SearchResults SearchResults { get; }
    }
}