using BR.Core.Caching;
using System;
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

        public static IEnumerable<T> NewIfNull<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.IsNull())
            {
                return new List<T>();
            }

            return enumerable;
        }

        public static IEnumerable<string> NonNullOrEmpty(this IEnumerable<string> strings)
        {
            return strings.Where(s => !(string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)));
        }

        public static IEnumerable<IEnumerable<T>> ToBatches<T>(
            this IEnumerable<T> items, int batchSize = 10)
        {
            for (int i = 0; i < items.Count(); i += batchSize)
            {
                yield return items.Skip(i).Take(batchSize);
            }
        }

        public static System.Collections.IEnumerable ToGenericEnumerable(
            this System.Collections.IEnumerable enumerable, Type type)
        {
            var genericList = Types.GenericList.MakeGenericType(type);
            var list = Activator.CreateInstance(genericList) as System.Collections.IList;

            foreach (var item in enumerable)
            {
                list.Add(item);
            }

            return list;
        }
    }
}
