using Assert.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class FuzzyMatcher : IFuzzyMatcher
    {
        public double Compare(string a, string b)
        {
            if (string.IsNullOrEmpty(a)) { return string.IsNullOrEmpty(b) ? 1 : 0; }

            if (string.IsNullOrEmpty(b)) return 0;

            // Usar algoritmo de Levenshtein modificado
            var distance = LevenshteinDistance(a, b);
            var maxLength = Math.Max(a.Length, b.Length);

            return 1.0 - (double)distance / maxLength;
        }

        private int LevenshteinDistance(string s, string t)
        {
            // Implementación optimizada del algoritmo de Levenshtein
            if (s == t) return 0;
            if (s.Length == 0) return t.Length;
            if (t.Length == 0) return s.Length;

            var costs = new int[t.Length + 1];

            for (var i = 0; i <= s.Length; i++)
            {
                var lastValue = i;
                for (var j = 0; j <= t.Length; j++)
                {
                    if (i == 0)
                        costs[j] = j;
                    else
                    {
                        if (j > 0)
                        {
                            var newValue = costs[j - 1];
                            if (s[i - 1] != t[j - 1])
                                newValue = Math.Min(Math.Min(newValue, lastValue), costs[j]) + 1;
                            costs[j - 1] = lastValue;
                            lastValue = newValue;
                        }
                    }
                }
                if (i > 0)
                    costs[t.Length] = lastValue;
            }
            return costs[t.Length];
        }
    }
}
