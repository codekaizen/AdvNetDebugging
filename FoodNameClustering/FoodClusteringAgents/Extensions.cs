using System;
using System.Collections.Generic;
using System.Text;

namespace Esha.Analysis.FoodClusteringAgents
{
    public static class Extensions
    {
        public static void Iter<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        ///     Creates a comma-separated string from the items in <paramref name="sequence" />.
        /// </summary>
        /// <param name="sequence">
        ///     The sequence to print as a comma separated string.
        /// </param>
        /// <param name="selector">
        ///     A selector function used to project items before printing into a string.
        /// </param>
        /// <typeparam name="T">
        ///     The type of items in the enumerable.
        /// </typeparam>
        /// <returns>
        ///     A string generated from the concatenation of them items in <paramref name="sequence" /> separated
        ///     by commas and spaces.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="sequence" /> is <see langword="null" />.
        /// </exception>
        public static String PrintAsCsv<T>(this IEnumerable<T> sequence, Func<T, Object> selector = null)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            var builder = new StringBuilder();
            selector = selector ?? (t => t);
            sequence.Iter(i => builder.Append(selector(i)).Append(", "));

            if (builder.Length >= 2)
            {
                builder.Length -= 2; // remove final ", "
            }

            return builder.ToString();
        }
    }
}