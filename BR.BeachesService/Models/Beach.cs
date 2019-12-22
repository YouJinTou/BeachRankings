using BR.Core.Abstractions;
using BR.Core.Tools;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BR.BeachesService.Models
{
    public class Beach : IAggregate
    {
        public Beach()
        {
        }

        public Beach(
            string name,
            string addedBy,
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
        {
            this.Name = Validator.ReturnOrThrowIfNullOrWhiteSpace(name);
            this.AddedOn = DateTime.UtcNow;
            this.LastUpdatedOn = DateTime.UtcNow;
            this.AddedBy = addedBy;
            this.Continent = Validator.ReturnOrThrowIfNullOrWhiteSpace(continent);
            this.Country = country;
            this.L1 = l1;
            this.L2 = l2;
            this.L3 = l3;
            this.L4 = l4;
            this.WaterBody = Validator.ReturnOrThrowIfNullOrWhiteSpace(waterBody);
            this.Coordinates = coordinates;
            this.Location = GetId(this);
            this.Id = this.Location;
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
            this.Score = Beach.CalculateScore(this);
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

            var id =
                $"{beach.Name}_" +
                $"{beach.Continent}_" +
                $"{beach.Country}_" +
                $"{beach.L1}_" +
                $"{beach.L2}_" +
                $"{beach.L3}_" +
                $"{beach.L4}_" +
                $"{beach.WaterBody}_";
            id = Regex.Replace(id, "_+", "_").TrimEnd('_').ToLower();

            return id;
        }

        public static double? CalculateScore(Beach beach)
        {
            Validator.ThrowIfNull(beach, "Beach is empty.");

            var scores = new double?[]
            {
                beach.SandQuality,
                beach.BeachCleanliness,
                beach.BeautifulScenery,
                beach.CrowdFree,
                beach.Infrastructure,
                beach.WaterVisibility,
                beach.LitterFree,
                beach.FeetFriendlyBottom,
                beach.SeaLifeDiversity,
                beach.CoralReef,
                beach.Snorkeling,
                beach.Kayaking,
                beach.Walking,
                beach.Camping,
                beach.LongTermStay
            };

            if (Validator.AllNull(scores))
            {
                return null;
            }

            return (double?)Math.Round((decimal)scores.Average(), 1);
        }

        public static Beach CreateNull()
        {
            return new Beach();
        }
    }
}
