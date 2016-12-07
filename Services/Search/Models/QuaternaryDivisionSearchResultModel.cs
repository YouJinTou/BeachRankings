namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class QuaternaryDivisionSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string PrimaryDivision { get; set; }

        public string SecondaryDivision { get; set; }

        public string TertiaryDivision { get; set; }

        public int BeachCount { get; set; }
    }
}