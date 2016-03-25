﻿using System;

namespace Coordinator
{
    public class BingSearchEngineImpl : ISearchEngineImpl
    {
        public Uri CreateQuery(String query)
        {
            return new Uri($"http://www.bing.com/search?q={Uri.EscapeDataString(query)}");
        }
    }
}