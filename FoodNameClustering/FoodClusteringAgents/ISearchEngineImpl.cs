using System;

namespace Esha.Analysis.FoodClusteringAgents
{
    public interface ISearchEngineImpl
    {
        Uri CreateQuery(String query);
    }
}