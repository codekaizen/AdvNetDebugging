namespace Esha.Analysis.FoodClusteringAgents
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