using BR.Core.Caching;
using System.Collections.Generic;

namespace BR.Core.Tools
{
    public static class Collection
    {
        public static IEnumerable<T> Combine<T>(params object[] items)
        {
            var combined = new List<T>();

            if (Validator.AllNull(items))
            {
                return combined;
            }

            foreach (var item in items)
            {
                if (Types.Enumerable.IsAssignableFrom(item.GetType()))
                {
                    combined.AddRange((IEnumerable<T>)item);
                }
                else
                {
                    combined.Add((T)item);
                }
            }

            return combined;
        }
    }
}
