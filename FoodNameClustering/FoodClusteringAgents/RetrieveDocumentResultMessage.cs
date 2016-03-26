namespace Esha.Analysis.FoodClusteringAgents
{
    public class RetrieveDocumentResultMessage : ResultMessage<RetrieveDocumentRequestMessage>
    {
        public RetrieveDocumentResultMessage(RetrieveDocumentRequestMessage request, SearchResultDocument searchResultDoc)
            : base(request)
        {
            SearchResultDoc = searchResultDoc;
        }

        public SearchResultDocument SearchResultDoc { get; }
    }
}