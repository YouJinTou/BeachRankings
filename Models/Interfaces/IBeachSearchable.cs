namespace BeachRankings.Models.Interfaces
{
    public interface IBeachSearchable : ISearchable
    {
        string Description { get; set; }
        
        string Address { get; set; }

        string Coordinates { get; set; }
    }
}
