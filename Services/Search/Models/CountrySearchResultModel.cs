namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class CountrySearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}