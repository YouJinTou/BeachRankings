using Amazon.DynamoDBv2.DataModel;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.DAL;
using BeachRankings.Core.Tools;
using System;
using System.Text.RegularExpressions;

namespace BeachRankings.Core.Models
{
    [DynamoDBTable("Beaches", LowerCamelCaseProperties = false)]
    public class Beach : IDbModel
    {
        public const BeachPartitionKey PrimaryPartitionKeyType = BeachPartitionKey.Beach;

        public Beach()
        {
        }

        public Beach(
            string id,
            string name, 
            string continent,
            string country,
            string l1,
            string l2,
            string l3,
            string l4,
            string waterBody,
            string coordinates)
        {
            this.Id = id;
            this.Name = InputValidator.ReturnOrThrowIfNullOrWhiteSpace(name);
            this.AddedOn = DateTime.UtcNow;
            this.Continent = InputValidator.ReturnOrThrowIfNullOrWhiteSpace(continent);
            this.Country = country;
            this.L1 = l1;
            this.L2 = l2;
            this.L3 = l3;
            this.L4 = l4;
            this.WaterBody = InputValidator.ReturnOrThrowIfNullOrWhiteSpace(waterBody);
            this.Coordinates = coordinates;
            this.Location = this.GetLocation();
        }

        public Beach(
            string id,
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
            : this(id, name, continent, country, l1, l2, l3, l4, waterBody, coordinates)
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

        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime AddedOn { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }

        public string WaterBody { get; set; }

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

        private string GetLocation()
        {
            var location =
                $"{this.Name}_" +
                $"{this.Continent}_" +
                $"{this.Country}_" +
                $"{this.L1}_" +
                $"{this.L2}_" +
                $"{this.L3}_" +
                $"{this.L4}_" +
                $"{this.WaterBody}_";
            location = Regex.Replace(location, "_+", "_").TrimEnd('_').ToLower();

            return location;
        }
    }
}
