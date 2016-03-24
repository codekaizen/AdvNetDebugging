namespace Coordinator
{
    public class CompareDocumentsRequestMessage
    {
        public CompareDocumentsRequestMessage(DocumentScore sourceScore, DocumentScore targetScore)
        {
            SourceScore = sourceScore;
            TargetScore = targetScore;
        }

        public DocumentScore SourceScore { get; }
        public DocumentScore TargetScore { get; }
    }
}