namespace BeachRankings.Models.Interfaces
{
    using System.Collections.Generic;

    public interface IDivisionSearchable : ISearchable
    {
        ICollection<Beach> Beaches { get; set; }   
    }
}