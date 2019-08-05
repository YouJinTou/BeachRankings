using System.Collections.Generic;
using System.Linq;

namespace BeachRankings.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection.Count() == 0;
        }
    }
}
