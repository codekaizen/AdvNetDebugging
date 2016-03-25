using System;
using System.Collections.Generic;
using System.Linq;

namespace Coordinator
{
    public abstract class VectorBase<TElement>
    {
        private readonly TElement[] _elements;

        protected VectorBase(IEnumerable<TElement> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            _elements = elements.ToArray();
        }

        public TElement this[Int32 index] => _elements[index];

        public Int32 Length => _elements.Length;
    }
}