using BR.Core.Models;
using BR.Core.Tools;
using BR.SearchService.Abstractions;
using BR.SearchService.Models;
using System.Collections.Generic;
using System.Linq;

namespace BR.SearchService.Processing
{
    internal class QueryResultsParser : IQueryResultsParser
    {
        private IDictionary<string, IDictionary<string, List<string>>> tokenPostingsByType;

        public QueryResultsParser()
        {
            this.tokenPostingsByType = new Dictionary<string, IDictionary<string, List<string>>>
            {
                { PlaceType.Continent.ToString(), new Dictionary<string, List<string>>() },
                { PlaceType.Country.ToString(), new Dictionary<string, List<string>>() },
                { PlaceType.L1.ToString(), new Dictionary<string, List<string>>() },
                { PlaceType.L2.ToString(), new Dictionary<string, List<string>>() },
                { PlaceType.L3.ToString(), new Dictionary<string, List<string>>() },
                { PlaceType.L4.ToString(), new Dictionary<string, List<string>>() },
                { PlaceType.WaterBody.ToString(), new Dictionary<string, List<string>>() },
                { PlaceType.Beach.ToString(), new Dictionary<string, List<string>>() }
            };
        }

        public IEnumerable<SearchResult> ParseQueryResults(IEnumerable<IndexEntry> entries)
        {
            foreach (var entry in entries)
            {
                foreach (var posting in entry.Postings)
                {
                    tokenPostingsByType[posting.Type].Add(
                        posting.Place, posting.BeachIds.ToList());
                }
            }

            var results = Collection.Combine<SearchResult>(
                this.GetMaxPostingsEntries(PlaceType.Continent),
                this.GetMaxPostingsEntries(PlaceType.Country),
                this.GetMaxPostingsEntries(PlaceType.L1),
                this.GetMaxPostingsEntries(PlaceType.L2),
                this.GetMaxPostingsEntries(PlaceType.L3),
                this.GetMaxPostingsEntries(PlaceType.L4),
                this.GetMaxPostingsEntries(PlaceType.Beach), 2);

            return results;
        }

        private IEnumerable<KeyValuePair<string, List<string>>> GetMaxPostingsEntries(
            PlaceType type, int take = 1)
        {
            var result = this.tokenPostingsByType[type.ToString()]
                .OrderByDescending(s => s.Value.Count)
                .Take(take);

            return result;
        }
    }
}
