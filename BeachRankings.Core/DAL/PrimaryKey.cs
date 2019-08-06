using BeachRankings.Core.Tools;

namespace BeachRankings.Core.DAL
{
    public class PrimaryKey
    {
        public PrimaryKey(string partitionKey, string sortKey)
        {
            this.PartitionKey = InputValidator.ReturnOrThrowIfNullOrWhiteSpace(partitionKey);
            this.SortKey = InputValidator.ReturnOrThrowIfNullOrWhiteSpace(sortKey);
        }

        public string PartitionKey { get; }

        public string SortKey { get; }
    }
}
