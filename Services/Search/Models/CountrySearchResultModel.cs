namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class CountrySearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Continent { get; set; }

        public int BeachCount { get; set; }
    }
}