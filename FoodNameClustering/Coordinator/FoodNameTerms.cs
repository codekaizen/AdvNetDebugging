using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Coordinator
{
    public class FoodNameTerms
    {
        private static readonly Regex _splitter = new Regex(@"\W", RegexOptions.IgnoreCase);
        private readonly HashSet<String> _terms = new HashSet<String>();

        public FoodNameTerms(String foodName)
        {
            if (foodName == null)
            {
                throw new ArgumentNullException(nameof(foodName));
            }

            var splitTerms = _splitter.Split(foodName);
            _terms.UnionWith(splitTerms.Where(t => !String.IsNullOrWhiteSpace(t)).Select(t => t.ToLowerInvariant()));
        }
    }
}