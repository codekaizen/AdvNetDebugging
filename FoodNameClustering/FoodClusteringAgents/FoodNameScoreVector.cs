using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Esha.Analysis.FoodClusteringAgents
{
    [DebuggerDisplay("{DebugView}")]
    public class FoodNameScoreVector : VectorBase<Double>
    {
        public FoodNameScoreVector(IEnumerable<Double> elements)
            : base(elements)
        { }

        public Double TotalScore => this.Sum();

        internal String DebugView => $"Score: {TotalScore} out of {Length} elements.";
    }
}