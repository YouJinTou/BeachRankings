using System.Collections.Generic;
using System.Linq;

namespace BR.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.IsNull())
            {
                return true;
            }

            return enumerable.Count() == 0;
        }
    }
}
