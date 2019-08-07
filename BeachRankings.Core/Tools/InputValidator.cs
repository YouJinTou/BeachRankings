using System;
using System.Linq;

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

        public static string ReturnOrThrowIfNullOrWhiteSpace(string str, string message = null)
        {
            ThrowIfNullOrWhiteSpace(str, message);

            return str;
        }

        public static void ThrowIfNotPositive(int number, string message = null)
        {
            if (number <= 0)
            {
                throw new ArgumentException(message ?? nameof(number));
            }
        }

        public static void ThrowIfAnyNotPositive(params int[] numbers)
        {
            ThrowIfNull(numbers);

            foreach (var number in numbers)
            {
                ThrowIfNotPositive(number);
            }
        }

        public static bool AllNullOrWhiteSpace(params string[] strings)
        {
            return strings?.All(s => string.IsNullOrWhiteSpace(s)) ?? true;
        }
    }
}
