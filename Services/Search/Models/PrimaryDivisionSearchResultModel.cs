namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class PrimaryDivisionSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }

        public int BeachCount { get; set; }
    }
}