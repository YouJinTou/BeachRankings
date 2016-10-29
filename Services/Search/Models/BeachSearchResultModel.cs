namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class BeachSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string PrimaryDivision { get; set; }

        public string SecondaryDivision { get; set; }

        public string TertiaryDivision { get; set; }

        public string QuaternaryDivision { get; set; }

        public string WaterBody { get; set; }

        public string Coordinates { get; set; }
    }
}