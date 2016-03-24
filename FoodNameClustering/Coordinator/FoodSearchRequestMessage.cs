using System;

namespace Coordinator
{
    public class FoodSearchRequestMessage
    {
        public TimeSpan TimeOut { get; set; }
        public String FoodName { get; set; }
        public Uri SearchUri { get; set; }
    }
}