using System;
using System.Collections.Generic;

namespace Esha.Analysis.FoodClusteringAgents
{
    public class DocumentVector : VectorBase<String>
    {
        public DocumentVector(IEnumerable<String> elements)
            : base(elements) { }
    }
}