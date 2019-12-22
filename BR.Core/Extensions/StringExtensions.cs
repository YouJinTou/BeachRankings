using System.Collections.Generic;

namespace BR.Core.Extensions
{
    public static class StringExtensions
    {
        public static string NullIfEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s) ? null : s;
        }

        public static void AddTo(this string s, params ICollection<string>[] collections)
        {
            foreach (var collection in collections)
            {
                collection.Add(s);
            }
        }
    }
}
