using Amazon.DynamoDBv2.DataModel;
using BR.Core.Abstractions;
using System.Collections.Generic;

namespace BR.Core.Models
{
    public class IndexEntry : IDbModel
    {
        [DynamoDBHashKey]
        public string Bucket { get; set; }

        [DynamoDBRangeKey]
        public string Token { get; set; }

        public IEnumerable<IndexPosting> Postings { get; set; }

        public override string ToString()
        {
            return $"{Bucket}|{Token}";
        }
    }
}
