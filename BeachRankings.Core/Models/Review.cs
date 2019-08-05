using Amazon.DynamoDBv2.DataModel;
using BeachRankings.Core.Abstractions;
using System;

namespace BeachRankings.Core.Models
{
    public class Review : IDbModel
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBRangeKey]
        public string BeachId { get; set; }

        public DateTime PostedOn { get; set; }

        public string Content { get; set; }

        public double? Score { get; set; }

        public double? SandQuality { get; set; }

        public double? BeachCleanliness { get; set; }

        public double? BeautifulScenery { get; set; }

        public double? CrowdFree { get; set; }

        public double? Infrastructure { get; set; }

        public double? WaterVisibility { get; set; }

        public double? LitterFree { get; set; }

        public double? FeetFriendlyBottom { get; set; }

        public double? SeaLifeDiversity { get; set; }

        public double? CoralReef { get; set; }

        public double? Snorkeling { get; set; }

        public double? Kayaking { get; set; }

        public double? Walking { get; set; }

        public double? Camping { get; set; }

        public double? LongTermStay { get; set; }
    }
}
