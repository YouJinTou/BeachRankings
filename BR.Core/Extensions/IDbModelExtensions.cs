using Amazon.DynamoDBv2.DocumentModel;
using BR.Core.Abstractions;
using BR.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BR.Core.Extensions
{
    internal static class IDbModelExtensions
    {
        public static Document ToDynamoDbDocument(this IDbModel model)
        {
            var properties = model.GetType().GetProperties();
            var document = new Document();

            foreach (var prop in properties)
            {
                document[prop.Name] = GetDynamoValue(model, prop);
            }

            return document;
        }

        private static DynamoDBEntry GetDynamoValue(IDbModel model, PropertyInfo prop)
        {
            var value = prop.GetValue(model);

            if (value == null)
            {
                return new DynamoDBNull();
            }

            switch (prop.PropertyType)
            {
                case var type when prop.PropertyType == Types.String:
                    return (string)value ?? Types.DynamoNull;
                case var type when prop.PropertyType == Types.Int:
                    return (int)value;
                case var type when prop.PropertyType == Types.Long:
                    return (long)value;
                case var type when prop.PropertyType == Types.DateTime:
                    return (DateTime)value;
                case var type when prop.PropertyType == Types.NullDateTime:
                    return (DateTime?)value;
                case var type when prop.PropertyType == Types.Guid:
                    return (Guid)value;
                case var type when prop.PropertyType == Types.NullDouble:
                    if ((double?)value == null)
                    {
                        return Types.DynamoNull;
                    }

                    return (double?)value;
                case var type when prop.PropertyType == Types.ByteArray:
                    return (byte[])value;
                case var type when prop.PropertyType == Types.StringEnumerable:
                    return ((IEnumerable<string>)value).ToArray();
                case var type when Types.Enumerable.IsAssignableFrom(prop.PropertyType):
                    return ((System.Collections.IEnumerable)value)
                        .OfType<IDbModel>()
                        .Select(m => ToDynamoDbDocument(m))
                        .ToList();
                default:
                    throw new InvalidOperationException("Could not determine Dynamo type.");
            }
        }
    }
}
