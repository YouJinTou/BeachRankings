using Amazon.DynamoDBv2.DocumentModel;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Factories;
using System;
using System.Reflection;

namespace BeachRankings.Core.Extensions
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
                return DynamoObjectsFactory.CreateNull();
            }

            switch (prop.PropertyType)
            {
                case var type when prop.PropertyType == typeof(string):
                    return (string)value ?? DynamoObjectsFactory.CreateNull();
                case var type when prop.PropertyType == typeof(DateTime):
                    if (((DateTime)value) == DateTime.MinValue)
                    {
                        return DynamoObjectsFactory.CreateNull();
                    }

                    return (DateTime)value;
                case var type when prop.PropertyType == typeof(Guid):
                    if ((Guid)value == Guid.Empty)
                    {
                        return DynamoObjectsFactory.CreateNull();
                    }

                    return (Guid)value;
                case var type when prop.PropertyType == typeof(double?):
                    return (double)value;
                default:
                    return DynamoObjectsFactory.CreateNull();
            }
        }
    }
}
