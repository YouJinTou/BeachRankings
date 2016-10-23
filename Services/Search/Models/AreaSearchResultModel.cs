namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class AreaSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}