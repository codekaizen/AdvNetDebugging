using System;
using System.Diagnostics;

namespace Esha.Analysis.FoodClusteringAgents
{
    [DebuggerDisplay("{ToString()} ({System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(this)})")]
    public class FoodSearchRequestMessage
    {
        public FoodSearchRequestMessage(Uri searchUri, String foodName, TimeSpan timeOut)
        {
            if (searchUri == null)
            {
                throw new ArgumentNullException(nameof(searchUri));
            }
            if (foodName == null)
            {
                throw new ArgumentNullException(nameof(foodName));
            }
            SearchUri = searchUri;
            FoodName = foodName;
            TimeOut = timeOut;
        }

        public TimeSpan TimeOut { get; }
        public String FoodName { get; }
        public Uri SearchUri { get; }
    }
}