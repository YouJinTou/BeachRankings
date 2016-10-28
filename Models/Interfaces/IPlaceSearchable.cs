namespace BeachRankings.Models.Interfaces
{
    using System.Collections.Generic;

    public interface IPlaceSearchable : ISearchable
    {
        ICollection<Beach> Beaches { get; }   
    }
}