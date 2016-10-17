namespace BeachRankings.Services.Search.Models
{
    using BeachRankings.Models.Interfaces;

    public class BeachSearchResultModel : ISearchable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LocationName { get; set; }

        public string WaterBody { get; set; }

        public string ApproximateAddress { get; set; }

        public string Coordinates { get; set; }
    }
}