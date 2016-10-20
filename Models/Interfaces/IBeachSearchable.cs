namespace BeachRankings.Models.Interfaces
{
    public interface IBeachSearchable : ISearchable
    {
        string WaterBodyName { get; set; }

        string Description { get; set; }
        
        string ApproximateAddress { get; set; }

        string Coordinates { get; set; }
    }
}
