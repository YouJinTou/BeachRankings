using Amazon.DynamoDBv2.DataModel;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Tools;
using System;

namespace BeachRankings.Core.Models
{
    [DynamoDBTable("Beaches", LowerCamelCaseProperties = false)]
    public class Beach : Place, IDbModel
    {
        public Beach()
        {
        }

        public Beach(
            string name, 
            string continent,
            string country,
            string l1,
            string l2,
            string l3,
            string l4,
            string waterBody,
            string coordinates)
            : base(continent, country, l1, l2, l3, l4, waterBody)
        {
            this.Name = InputValidator.ReturnOrThrowIfNullOrWhiteSpace(name);
            this.AddedOn = DateTime.UtcNow;
            this.Coordinates = coordinates;
            this.Id = this.GetId();
            this.Location = this.GetKey();
        }

        public Beach(
            string name,
            string continent,
            string country,
            string l1,
            string l2,
            string l3,
            string l4,
            string waterBody,
            string coordinates,
            double? score,
            double? sandQuality,
            double? beachCleanliness,
            double? beautifulScenery,
            double? crowdFree,
            double? infrastructure,
            double? waterVisibility,
            double? litterFree,
            double? feetFriendlyBottom,
            double? seaLifeDiversity,
            double? coralReef,
            double? snorkeling,
            double? kayaking,
            double? walking,
            double? camping,
            double? longTermStay)
            : this(name, continent, country, l1, l2, l3, l4, waterBody, coordinates)
        {
            this.Score = score;
            this.SandQuality = sandQuality;
            this.BeachCleanliness = beachCleanliness;
            this.BeautifulScenery = beautifulScenery;
            this.CrowdFree = crowdFree;
            this.Infrastructure = infrastructure;
            this.WaterVisibility = waterVisibility;
            this.LitterFree = litterFree;
            this.FeetFriendlyBottom = feetFriendlyBottom;
            this.SeaLifeDiversity = seaLifeDiversity;
            this.CoralReef = coralReef;
            this.Snorkeling = snorkeling;
            this.Kayaking = kayaking;
            this.Walking = walking;
            this.Camping = camping;
            this.LongTermStay = longTermStay;
        }

        public string Name { get; set; }

        public DateTime AddedOn { get; set; }

        [DynamoDBRangeKey]
        public string Location { get; set; }

        public string Coordinates { get; set; }

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

        protected override string GetId()
        {
            return $"id_{this.GetKey()}";
        }

        protected override string GetKey()
        {
            return base.GetKey() + $"_{this.Name}";
        }
    }
}
