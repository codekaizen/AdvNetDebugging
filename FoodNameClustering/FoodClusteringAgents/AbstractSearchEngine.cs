using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class AbstractSearchEngine
    {
        private readonly ISearchEngineImpl _searchEngineImpl;

        public AbstractSearchEngine(ISearchEngineImpl searchEngineImpl)
        {
            if (searchEngineImpl == null)
            {
                throw new ArgumentNullException(nameof(searchEngineImpl));
            }

            _searchEngineImpl = searchEngineImpl;
        }

        public Uri CreateQuery(String query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return _searchEngineImpl.CreateQuery(query);
        }
    }
}