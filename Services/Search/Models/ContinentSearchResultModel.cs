namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class ContinentSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BeachCount { get; set; }
    }
}