using BeachRankings.Models.Interfaces;
using System.Collections.Generic;

namespace BeachRankings.Services.Search
{
    public interface ISearchService
    {
        IEnumerable<ISearchable> SearchByPrefix(string prefix, int maxItems);

        IBeachAggregatable SearchByBeachId(int beachId);

        ICollection<IBeachAggregatable> SearchByContinentId(int continentId);

        ICollection<IBeachAggregatable> SearchByCountryId(int countryId);

        ICollection<IBeachAggregatable> SearchByPrimaryDivisionId(int primaryDivisionId);

        ICollection<IBeachAggregatable> SearchByWaterBodyId(int waterBodyId);

        void AddUpdateIndexEntry(ISearchable searchable);

        void AddUpdateIndexEntries(IEnumerable<ISearchable> searchables);

        void DeleteIndexEntry(ISearchable searchable);

        bool ClearIndex();
    }
}
