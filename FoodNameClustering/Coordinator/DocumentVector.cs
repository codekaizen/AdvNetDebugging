using System;
using System.Collections.Generic;

namespace Coordinator
{
    public class DocumentVector : VectorBase<String>
    {
        public DocumentVector(IEnumerable<String> elements)
            : base(elements) { }
    }
}