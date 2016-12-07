namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class QuaternaryDivisionSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }

        public string PrimaryDivisionName { get; set; }

        public string SecondaryDivisionName { get; set; }

        public string TertiaryDivisionName { get; set; }

        public int BeachCount { get; set; }
    }
}