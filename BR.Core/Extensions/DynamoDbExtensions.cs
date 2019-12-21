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

            switch (type)
            {
                case var x when type == Types.String:
                    return entry.AsString();
                case var x when type == Types.Int:
                    return entry.AsInt();
                case var x when type == Types.Int64:
                    return entry.AsLong();
                case var x when type == Types.DateTime:
                case var y when type == Types.NullDateTime:
                    return entry.AsDateTime();
                case var x when type == Types.NullDouble:
                    return entry.AsDouble();
                case var x when type == Types.ByteArray:
                    return entry.AsByteArray();
                case var x when type == Types.Guid:
                    return entry.AsGuid();
                default:
                    return entry.AsString();
            }
        }

        private static object GetValue(AttributeValue value)
        {
            return null;
        }
    }
}
