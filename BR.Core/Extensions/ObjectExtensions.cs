using System.Collections.Generic;

namespace BR.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static IEnumerable<T> AsEnumerable<T>(this T obj)
        {
            return new List<T> { obj };
        }
    }
}
