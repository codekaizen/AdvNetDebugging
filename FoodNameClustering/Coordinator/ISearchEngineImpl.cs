using System;

namespace Coordinator
{
    public interface ISearchEngineImpl
    {
        Uri CreateQuery(String query);
    }
}