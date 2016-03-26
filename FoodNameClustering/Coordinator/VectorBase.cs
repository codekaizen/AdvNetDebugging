using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Coordinator
{
    [DebuggerDisplay("{ToAbbreviatedString()}")]
    [DebuggerTypeProxy(typeof(VectorBase<>.DebugProxy))]
    public abstract class VectorBase<TElement> : IReadOnlyList<TElement>
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

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        /// The number of elements in the collection. 
        /// </returns>
        Int32 IReadOnlyCollection<TElement>.Count => Length;

        public Int32 Length => _elements.Length;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            return ((IEnumerable<TElement>)_elements).GetEnumerator();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override String ToString()
        {
            return $"[{_elements.PrintAsCsv()}]";
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public String ToAbbreviatedString()
        {
            if (Length < 7)
            {
                return ToString();
            }

            var first = new ArraySegment<TElement>(_elements, 0, 2);
            var last = new ArraySegment<TElement>(_elements, Length - 3, 2);
            return $"[{first.PrintAsCsv()}..({Length - 4} hidden elements)..{last.PrintAsCsv()}]";
        }

        private class DebugProxy
        {
            private readonly VectorBase<TElement> _instance;

            public DebugProxy(VectorBase<TElement> instance)
            {
                Debug.Assert(instance != null);
                _instance = instance;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public TElement[] Elements => _instance._elements;
        }
    }
}