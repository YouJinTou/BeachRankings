using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BR.Core.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BR.Core.Extensions
{
    internal static class DynamoDbExtensions
    {
        public static T ConvertTo<T>(this Document document)
        {
            var type = typeof(T);
            var properties = type.GetProperties().Select(p => p).ToList();
            var instance = Activator.CreateInstance<T>();

            foreach (var prop in properties)
            {
                var value = GetValue(document[prop.Name], prop.PropertyType);

                prop.SetValue(instance, value);
            }

            return instance;
        }

        public static T ConvertTo<T>(this Dictionary<string, AttributeValue> values)
        {
            var type = typeof(T);
            var properties = type.GetProperties().Select(p => p).ToList();
            var instance = Activator.CreateInstance<T>();

            foreach (var prop in properties)
            {
                var value = GetValue(values[prop.Name]);

                prop.SetValue(instance, value);
            }

            return instance;
        }

        private static object GetValue(DynamoDBEntry entry, Type type)
        {
            if (entry is DynamoDBNull)
            {
                return null;
            }

            if (type == Types.String)
            {
                return entry.AsString();
            }
            else if (type == Types.Int)
            {
                return entry.AsInt();
            }
            else if (type == Types.Int64)
            {
                return entry.AsLong();
            }
            else if (type == Types.DateTime)
            {
                return entry.AsDateTime();
            }
            else if (type == Types.NullDouble)
            {
                return entry.AsDouble();
            }
            else if (type == Types.ByteArray)
            {
                return entry.AsByteArray();
            }
            else if (type == Types.Guid)
            {
                return entry.AsGuid();
            }

            return entry.AsString();
        }

        private static object GetValue(AttributeValue value)
        {
            return null;
        }
    }
}
