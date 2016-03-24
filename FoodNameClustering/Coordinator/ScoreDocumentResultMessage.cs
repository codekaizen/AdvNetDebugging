namespace Coordinator
{
    public class ScoreDocumentResultMessage : ResultMessage<ScoreDocumentRequestMessage>
    {
        public ScoreDocumentResultMessage(ScoreDocumentRequestMessage request, DocumentScore score)
            : base(request)
        {
            Score = score;
        }

        public DocumentScore Score { get; }
    }
}