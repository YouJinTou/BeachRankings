using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Tools;
using System;
using System.Text.RegularExpressions;

namespace BR.Core.Models
{
    public class Beach : IAggregate, IScorable
    {
        public Beach()
        {
        }

        public Beach(
            string name,
            string continent,
            string waterBody,
            string country = null,
            string l1 = null,
            string l2 = null,
            string l3 = null,
            string l4 = null,
            string addedBy = null,
            string coordinates = null,
            double? score = null,
            double? sandQuality = null,
            double? beachCleanliness = null,
            double? beautifulScenery = null,
            double? crowdFree = null,
            double? infrastructure = null,
            double? waterVisibility = null,
            double? litterFree = null,
            double? feetFriendlyBottom = null,
            double? seaLifeDiversity = null,
            double? coralReef = null,
            double? snorkeling = null,
            double? kayaking = null,
            double? walking = null,
            double? camping = null,
            double? longTermStay = null)
        {
            this.Name = Validator.ReturnOrThrowIfNullOrWhiteSpace(name);
            this.Continent = Validator.ReturnOrThrowIfNullOrWhiteSpace(continent);
            this.WaterBody = Validator.ReturnOrThrowIfNullOrWhiteSpace(waterBody);
            this.AddedOn = DateTime.UtcNow;
            this.LastUpdatedOn = DateTime.UtcNow;
            this.AddedBy = addedBy;
            this.Country = country;
            this.L1 = l1;
            this.L2 = l2;
            this.L3 = l3;
            this.L4 = l4;
            this.Coordinates = coordinates;
            this.Location = GetLocation(this);
            this.Id = GetId(this);
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
            this.Score = this.CalculateScore();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime LastUpdatedOn { get; set; }

        public string AddedBy { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }

        public string WaterBody { get; set; }

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

        public static string GetId(Beach beach)
        {
            Validator.ThrowIfNull(beach, "Beach is empty.");

            var hashedId = Hasher.GetHash(GetLocation(beach));
            var id = Regex.Replace(hashedId, @"[^a-zA-Z]+", string.Empty);

            return id;
        }

        public static string GetLocation(Beach beach)
        {
            Validator.ThrowIfNull(beach, "Beach is empty.");

            var location =
                $"{beach.Name}_" +
                $"{beach.Continent}_" +
                $"{beach.Country}_" +
                $"{beach.L1}_" +
                $"{beach.L2}_" +
                $"{beach.L3}_" +
                $"{beach.L4}_" +
                $"{beach.WaterBody}_";
            location = Regex.Replace(location, "_+", "_").TrimEnd('_').ToLower();

            return location;
        }

        public static Beach CreateNull()
        {
            return new Beach();
        }
    }
}
