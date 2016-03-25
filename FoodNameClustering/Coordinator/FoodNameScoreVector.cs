using System;
using System.Collections.Generic;

namespace Coordinator
{
    public class FoodNameScoreVector : VectorBase<Double>
    {
        public FoodNameScoreVector(IEnumerable<Double> elements)
            : base(elements)
        { }
    }
}