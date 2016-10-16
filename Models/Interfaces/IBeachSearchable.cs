namespace BeachRankings.Models.Interfaces
{
    public interface IBeachSearchable : ISearchable
    {
        int LocationId { get; set; }

        string Description { get; set; }

        string WaterBody { get; set; }

        string ApproximateAddress { get; set; }

        string Coordinates { get; set; }
    }
}
