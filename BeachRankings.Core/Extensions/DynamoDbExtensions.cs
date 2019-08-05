using Amazon.DynamoDBv2.DocumentModel;
using BeachRankings.Core.Abstractions;
using System;
using System.Linq;

namespace BeachRankings.Core.Extensions
{
    internal static class DynamoDbExtensions
    {
        public static T ConvertTo<T>(this Document document) where T : IDbModel
        {
            var type = typeof(T);
            var properties = type.GetProperties().Select(p => p).ToList();
            var instance = Activator.CreateInstance<T>();

            foreach (var prop in properties)
            {
                prop.SetValue(instance, document[prop.Name].GetValue(prop.PropertyType));
            }

            return instance;
        }

        public static object GetValue(this DynamoDBEntry entry, Type type)
        {
            if (entry is DynamoDBNull)
            {
                return null;
            }

            if (type == typeof(string))
            {
                return entry.AsString();
            }
            else if (type == typeof(DateTime))
            {
                return entry.AsDateTime();
            }
            else if (type == typeof(double?))
            {
                return entry.AsDouble();
            }
            
            return entry.AsString();
        }
    }
}
