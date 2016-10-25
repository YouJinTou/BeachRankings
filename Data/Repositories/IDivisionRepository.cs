namespace BeachRankings.Data.Repositories
{
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IDivisionRepository
    {
        IDivision Find(Type type, object id);

        IDivision Add(IDivision division);

        void Remove(IDivision division);

        void SaveChanges();

        IQueryable<PrimaryDivision> PrimaryDivisions();

        IQueryable<SecondaryDivision> SecondaryDivisions();

        IQueryable<TertiaryDivision> TertiaryDivisions();

        IQueryable<QuaternaryDivision> QuaternaryDivisions();

        IEnumerable<ISearchable> GetSearchResultsByKeyStroke(string prefix);
       
        void AddToIndex(ISearchable searchable);
    }
}