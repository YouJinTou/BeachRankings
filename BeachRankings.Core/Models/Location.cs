using Amazon.DynamoDBv2.DataModel;
using BeachRankings.Core.Abstractions;

namespace BeachRankings.Core.Models
{
    [DynamoDBTable("Locations", LowerCamelCaseProperties = false)]
    public class Location : Place, IDbModel
    {
        public Location(
           string continent,
           string country,
           string l1,
           string l2,
           string l3,
           string l4,
           string waterBody)
           : base(continent, country, l1, l2, l3, l4, waterBody)
        {
            this.Key = this.GetKey();
        }

        [DynamoDBRangeKey]
        public string Key { get; set; }
    }
}
