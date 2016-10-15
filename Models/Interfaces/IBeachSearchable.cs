namespace BeachRankings.Models.Interfaces
{
    public interface IBeachSearchable : ISearchable
    {
        string Location { get; set; }

        string Description { get; set; }

        string WaterBody { get; set; }

        string ApproximateAddress { get; set; }

        string Coordinates { get; set; }
    }
}
