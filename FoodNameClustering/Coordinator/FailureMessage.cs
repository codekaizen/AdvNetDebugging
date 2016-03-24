using System;

namespace Coordinator
{
    public class FailureMessage<TRequest>
    {
        public FailureMessage(TRequest request, Exception exception = null)
        {
            Request = request;
            Exception = exception;
        }

        public TRequest Request { get; }
        public Exception Exception { get; }
    }
}