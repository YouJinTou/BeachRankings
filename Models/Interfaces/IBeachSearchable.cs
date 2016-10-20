namespace BeachRankings.Models.Interfaces
{
    public interface IBeachSearchable : ISearchable
    {
        int CountryId { get; set; }

        int LocationId { get; set; }

        int WaterBodyId { get; set; }

        string WaterBodyName { get; set; }

        string Description { get; set; }
        
        string ApproximateAddress { get; set; }

        string Coordinates { get; set; }
    }
}
