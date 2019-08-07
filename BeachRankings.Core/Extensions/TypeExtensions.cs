using BeachRankings.Core.Tools;
using System;
using System.Reflection;

namespace BeachRankings.Core.Extensions
{
    public static class TypeExtensions
    {
        public static PropertyInfo TryGetProperty(
            this Type type, string propertyName, string fallbackProperty)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                InputValidator.ThrowIfNullOrWhiteSpace(fallbackProperty);

                return type.GetProperty(fallbackProperty);
            }

            var property = type.GetProperty(propertyName);

            return property ?? throw new InvalidOperationException("Invalid property");
        }
    }
}
