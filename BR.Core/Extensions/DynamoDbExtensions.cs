using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BR.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BR.Core.Extensions
{
    public static class DynamoDbExtensions
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

        public static object ConvertTo(this Document document, Type type)
        {
            var properties = type.GetProperties().Select(p => p).ToList();
            var instance = Activator.CreateInstance(type);

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
                if (values.ContainsKey(prop.Name))
                {
                    var value = GetValue(values[prop.Name], prop.PropertyType);

                    prop.SetValue(instance, value);
                }
            }

            return instance;
        }

        public static object ConvertTo(this Dictionary<string, AttributeValue> values, Type type)
        {
            var properties = type.GetProperties().Select(p => p).ToList();
            var instance = Activator.CreateInstance(type);

            foreach (var prop in properties)
            {
                if (values.ContainsKey(prop.Name))
                {
                    var value = GetValue(values[prop.Name], prop.PropertyType);

                    prop.SetValue(instance, value);
                }
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
                case var _ when type == Types.String:
                    return entry.AsString();
                case var _ when type == Types.Int:
                    return entry.AsInt();
                case var _ when type == Types.Int64:
                    return entry.AsLong();
                case var _ when type == Types.DateTime:
                case var y when type == Types.NullDateTime:
                    return entry.AsDateTime();
                case var _ when type == Types.NullDouble:
                    return entry.AsDouble();
                case var _ when type == Types.ByteArray:
                    return entry.AsByteArray();
                case var _ when type == Types.Guid:
                    return entry.AsGuid();
                case var _ when type == Types.StringEnumerable:
                    return entry.AsListOfString();
                case var _ when Types.Enumerable.IsAssignableFrom(type):
                    return entry.AsListOfDocument()
                        .Select(d => ConvertTo(d, type.GenericTypeArguments[0]))
                        .ToGenericEnumerable(type.GenericTypeArguments[0]);
                default:
                    throw new NotImplementedException("Dynamo type not implemented.");
            }
        }

        private static object GetValue(AttributeValue value, Type type)
        {
            if (type == Types.String)
            {
                return value.S;
            }

            if (type == Types.NullDouble)
            {
                return value.S.ToNullDouble();
            }

            if (!value.SS.IsNullOrEmpty())
            {
                return value.SS;
            }

            if (!value.L.IsNullOrEmpty())
            {
                return value.L.Select(i => GetValue(i, type.GenericTypeArguments[0]))
                    .ToGenericEnumerable(type.GenericTypeArguments[0]);
            }

            if (!value.M.IsNullOrEmpty())
            {
                return value.M.ConvertTo(type);
            }

            throw new InvalidOperationException("Cannot get attribute value.");
        }
    }
}
