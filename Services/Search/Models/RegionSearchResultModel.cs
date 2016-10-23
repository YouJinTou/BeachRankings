namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class RegionSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}