namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class SecondaryDivisionSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string PrimaryDivision { get; set; }

        public int BeachCount { get; set; }
    }
}