using BeachRankings.Core.DAL;

namespace BeachRankings.Core.Factories
{
    public static class DalObjectsFactory
    {
        public static PrimaryKey CreatePrimaryKey(string partitionKey, string sortKey)
        {
            return new PrimaryKey(partitionKey, sortKey);
        }
    }
}
