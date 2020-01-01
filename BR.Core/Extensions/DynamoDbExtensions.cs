using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using BR.Core.Caching;
using System;
using System.Collections;
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

        public static string AsBucket(this string s)
        {
            return string.Join(string.Empty, s.Take(2));
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
                case var x when type == Types.StringEnumerable:
                    return entry.AsListOfString();
                default:
                    throw new NotImplementedException("Dynamo type not implemented.");
            }
        }

        private static object GetValue(AttributeValue value, Type type)
        {
            if (!string.IsNullOrWhiteSpace(value.S))
            {
                return value.S;
            }

            if (!value.SS.IsNullOrEmpty())
            {
                return value.SS;
            }

            if (!value.L.IsNullOrEmpty())
            {
                var genericList = Types.GenericList.MakeGenericType(type.GenericTypeArguments[0]);
                var list = (Activator.CreateInstance(genericList)) as IList;

                foreach (var item in value.L)
                {
                    var result = GetValue(item, type.GenericTypeArguments[0]);

                    list.Add(result);
                }

                return list;
            }

            if (!value.M.IsNullOrEmpty())
            {
                return value.M.ConvertTo(type);
            }

            throw new InvalidOperationException();
        }
    }
}
