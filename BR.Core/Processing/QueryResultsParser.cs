using BR.Core.Abstractions;
using BR.Core.Models;
using BR.Core.Tools;
using System.Collections.Generic;
using System.Linq;

namespace BR.Core.Processing
{
    internal class QueryResultsParser : IQueryResultsParser
    {
        private IDictionary<string, IList<IndexPosting>> postingsItemByType;

        public QueryResultsParser()
        {
            this.postingsItemByType = new Dictionary<string, IList<IndexPosting>>
            {
                { PlaceType.Continent.ToString(), new List<IndexPosting>() },
                { PlaceType.Country.ToString(), new List<IndexPosting>() },
                { PlaceType.L1.ToString(), new List<IndexPosting>() },
                { PlaceType.L2.ToString(), new List<IndexPosting>() },
                { PlaceType.L3.ToString(), new List<IndexPosting>() },
                { PlaceType.L4.ToString(), new List<IndexPosting>() },
                { PlaceType.WaterBody.ToString(), new List<IndexPosting>() },
                { PlaceType.Beach.ToString(), new List<IndexPosting>() }
            };
        }

        public IEnumerable<SearchResult> ParseQueryResults(IEnumerable<IndexEntry> entries)
        {
            foreach (var entry in entries)
            {
                foreach (var posting in entry.Postings)
                {
                    this.postingsItemByType[posting.Type].Add(posting);
                }
            }

            var results = Collection.Combine<SearchResult>(
                this.GetSearchResults(PlaceType.Continent),
                this.GetSearchResults(PlaceType.Country),
                this.GetSearchResults(PlaceType.L1),
                this.GetSearchResults(PlaceType.L2),
                this.GetSearchResults(PlaceType.L3),
                this.GetSearchResults(PlaceType.L4),
                this.GetSearchResults(PlaceType.Beach, 2),
                this.GetSearchResults(PlaceType.WaterBody, 2));

            return results;
        }

        private IEnumerable<SearchResult> GetSearchResults(
            PlaceType type, int take = 1)
        {
            var results = this.postingsItemByType[type.ToString()]
            .Select(i => new SearchResult
            {
                Id = i.Id,
                Name = i.Place,
                Type = i.Type,
                BeachesCount = i.Beaches.Count()
            })
            .OrderByDescending(r => r.BeachesCount)
            .Take(take)
            .ToList();

            return results;
        }
    }
}
