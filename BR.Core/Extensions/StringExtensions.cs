using BR.Core.Tools;
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

        public static double? ToNullDouble(this string number, string defaultNullString = null)
        {
            var isNull = string.
                IsNullOrEmpty(number) || 
                number.ToLower().Equals(defaultNullString?.ToLower());

            return isNull ? (double?)null : double.Parse(number);
        }

        public static string NullIfNullString(this string s, string defaultNullString = null)
        {
            return 
                string.IsNullOrWhiteSpace(s) || s.ToLower().Equals(defaultNullString?.ToLower()) ?
                null : 
                s;
        }

        public static string Latinize(this string s)
        {
            return Latinizer.Latinize(s);
        }
    }
}
