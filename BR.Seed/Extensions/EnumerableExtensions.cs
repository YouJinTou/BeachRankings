using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Seed.Extensions
{
    internal static class EnumerableExtensions
    {
        public static async Task<IEnumerable<U>> SelectDelayAsync<T, U>(
            this IEnumerable<T> items, Func<T, U> func)
        {
            var projection = new List<U>();

            foreach (var item in items)
            {
                await Task.Delay(1);

                projection.Add(func(item));
            }

            return projection;
        }
    }
}
