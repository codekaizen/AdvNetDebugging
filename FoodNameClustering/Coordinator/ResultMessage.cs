namespace Coordinator
{
    public class ResultMessage<TRequest>
    {
        protected ResultMessage(TRequest request)
        {
            Request = request;
        }

        public TRequest Request { get; }
    }
}