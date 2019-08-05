using System;

namespace BeachRankings.Core.Tools
{
    public static class InputValidator
    {
        public static void ThrowIfNull(object obj, string message = null)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(message ?? nameof(obj));
            }
        }

        public static void ThrowIfNullOrWhiteSpace(string str, string message = null)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException(message ?? nameof(str));
            }
        }

        public static string ReturnOrThrowIfNullOrWhiteSpace(string str)
        {
            ThrowIfNullOrWhiteSpace(str);

            return str;
        }
    }
}
