namespace Coordinator
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