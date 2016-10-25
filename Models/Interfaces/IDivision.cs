namespace BeachRankings.Models.Interfaces
{
    using System.Collections.Generic;

    public interface IDivision : ISearchable
    {
        ICollection<Beach> Beaches { get; set; }   
    }
}