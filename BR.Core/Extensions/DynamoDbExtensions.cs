using Amazon.DynamoDBv2.DocumentModel;
using BR.Core.System;
using System;
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

            if (type == Types.String)
            {
                return entry.AsString();
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
    }
}
