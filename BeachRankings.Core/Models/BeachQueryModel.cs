using BeachRankings.Core.Tools;

namespace BeachRankings.Core.Models
{
    public class BeachQueryModel
    {
        public string Prefix { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }

        public string WaterBody { get; set; }

        public string OrderBy { get; set; }

        public string SortDirection { get; set; }

        public bool IsValid()
        {
            return !InputValidator.AllNullOrWhiteSpace(
                this.Prefix,
                this.Continent,
                this.Country,
                this.L1,
                this.L2,
                this.L3,
                this.L4,
                this.WaterBody,
                this.OrderBy,
                this.SortDirection);
        }
    }
}
