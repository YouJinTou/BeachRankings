using Amazon.DynamoDBv2.DocumentModel;

namespace BeachRankings.Core.Factories
{
    internal static class DynamoObjectsFactory
    {
        public static DynamoDBNull CreateNull()
        {
            return new DynamoDBNull();
        }
    }
}
