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
            var property = type.GetProperty(propertyName);

            if (property == null)
            {
                InputValidator.ThrowIfNullOrWhiteSpace(fallbackProperty);

                return type.GetProperty(fallbackProperty);
            }

            return property;
        }
    }
}
