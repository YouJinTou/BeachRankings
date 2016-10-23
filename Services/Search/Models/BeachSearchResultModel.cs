namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class BeachSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string Coordinates { get; set; }
    }
}