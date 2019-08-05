using Amazon.DynamoDBv2.DataModel;
using BeachRankings.Core.Tools;
using System.Text.RegularExpressions;

namespace BeachRankings.Core.Models
{
    public abstract class Place
    {
        internal Place()
        {
        }

        public Place(
            string continent,
            string country,
            string l1,
            string l2,
            string l3,
            string l4,
            string waterBody)
        {
            this.Continent = InputValidator.ReturnOrThrowIfNullOrWhiteSpace(continent);
            this.Country = country;
            this.L1 = l1;
            this.L2 = l2;
            this.L3 = l3;
            this.L4 = l4;
            this.WaterBody = InputValidator.ReturnOrThrowIfNullOrWhiteSpace(waterBody);
            this.Id = this.GetId();
        }

        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }

        public string WaterBody { get; set; }

        protected virtual string GetId()
        {
            return $"id_{this.GetKey()}";
        }

        protected virtual string GetKey()
        {
            var key =
                $"{this.Continent}_" +
                $"{this.Country}_" +
                $"{this.L1}_" +
                $"{this.L2}_" +
                $"{this.L3}_" +
                $"{this.L4}_" +
                $"{this.WaterBody}";

            return Regex.Replace(key, "_+", "_").TrimEnd('_');
        }
    }
}
