using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Coordinator
{
    public sealed class FoodNameTerms : IEquatable<FoodNameTerms>
    {
        private static readonly Regex _splitter = new Regex(@"\W", RegexOptions.IgnoreCase);
        private readonly HashSet<String> _terms = new HashSet<String>(StringComparer.CurrentCultureIgnoreCase);

        public FoodNameTerms(String foodName)
        {
            if (foodName == null)
            {
                throw new ArgumentNullException(nameof(foodName));
            }

            var splitTerms = _splitter.Split(foodName);
            _terms.UnionWith(splitTerms.Where(t => !String.IsNullOrWhiteSpace(t)).Select(t => t.ToLowerInvariant()));
        }

        public Boolean Contains(String term)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            return _terms.Contains(term);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(FoodNameTerms other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _terms.SetEquals(other._terms);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as FoodNameTerms);
        }

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return _terms.Aggregate(0, (agg, t) => agg + t.GetHashCode());
            }
        }

        public static bool operator ==(FoodNameTerms left, FoodNameTerms right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FoodNameTerms left, FoodNameTerms right)
        {
            return !Equals(left, right);
        }
    }
}