using Amazon.DynamoDBv2.DataModel;
using BR.Core.Abstractions;
using System.Collections.Generic;

namespace BR.Core.Models
{
    public class Place : IDbModel
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public IEnumerable<string> Children { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }

        public IEnumerable<string> WaterBodies { get; set; }
    }
}
