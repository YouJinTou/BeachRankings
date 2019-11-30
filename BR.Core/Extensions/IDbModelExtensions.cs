using Amazon.DynamoDBv2.DocumentModel;
using BR.Core.Abstractions;
using System;
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
                case var type when prop.PropertyType == typeof(string):
                    return (string)value ?? new DynamoDBNull();
                case var type when prop.PropertyType == typeof(DateTime):
                    if (((DateTime)value) == DateTime.MinValue)
                    {
                        return new DynamoDBNull();
                    }

                    return (DateTime)value;
                case var type when prop.PropertyType == typeof(Guid):
                    if ((Guid)value == Guid.Empty)
                    {
                        return new DynamoDBNull();
                    }

                    return (Guid)value;
                case var type when prop.PropertyType == typeof(double?):
                    return (double)value;
                default:
                    return new DynamoDBNull();
            }
        }
    }
}
