using Amazon.DynamoDBv2.DocumentModel;
using BR.Core.Abstractions;
using System;
using System.Reflection;

namespace BR.Core.Extensions
{
    internal static class IDbModelExtensions
    {
        private static Type stringType = typeof(string);
        private static Type dateTimeType = typeof(DateTime);
        private static Type guidType = typeof(Guid);
        private static Type nullDoubleType = typeof(double?);
        private static Type byteArrayType = typeof(byte[]);
        private static DynamoDBNull dynamoNull = new DynamoDBNull();

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
                case var type when prop.PropertyType == stringType:
                    return (string)value ?? dynamoNull;
                case var type when prop.PropertyType == dateTimeType:
                    if (((DateTime)value) == DateTime.MinValue)
                    {
                        return dynamoNull;
                    }

                    return (DateTime)value;
                case var type when prop.PropertyType == guidType:
                    if ((Guid)value == Guid.Empty)
                    {
                        return dynamoNull;
                    }

                    return (Guid)value;
                case var type when prop.PropertyType == nullDoubleType:
                    if ((double?)value == null)
                    {
                        return dynamoNull;
                    }

                    return (double?)value;
                case var type when prop.PropertyType == typeof(byte[]):
                    return (byte[])value;
                default:
                    return dynamoNull;
            }
        }
    }
}
