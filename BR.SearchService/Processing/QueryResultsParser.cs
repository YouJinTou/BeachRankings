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
        private class TokenPostingsItem
        {
            public string Id { get; set; }

            public IDictionary<string, List<string>> PostingsByToken { get; set; } 
                = new Dictionary<string, List<string>>();
        }

        private IDictionary<string, TokenPostingsItem> postingsItemByType;

        public QueryResultsParser()
        {
            this.postingsItemByType = new Dictionary<string, TokenPostingsItem>
            {
                { PlaceType.Continent.ToString(), new TokenPostingsItem() },
                { PlaceType.Country.ToString(), new TokenPostingsItem() },
                { PlaceType.L1.ToString(), new TokenPostingsItem() },
                { PlaceType.L2.ToString(), new TokenPostingsItem() },
                { PlaceType.L3.ToString(), new TokenPostingsItem() },
                { PlaceType.L4.ToString(), new TokenPostingsItem() },
                { PlaceType.WaterBody.ToString(), new TokenPostingsItem() },
                { PlaceType.Beach.ToString(), new TokenPostingsItem() }
            };
        }

        public IEnumerable<SearchResult> ParseQueryResults(IEnumerable<IndexEntry> entries)
        {
            foreach (var entry in entries)
            {
                foreach (var posting in entry.Postings)
                {
                    var item = postingsItemByType[posting.Type];
                    item.Id = entry.ToString();

                    item.PostingsByToken.Add(posting.Place, posting.BeachIds.ToList());
                }
            }

            var results = Collection.Combine<SearchResult>(
                this.GetSearchResults(PlaceType.Continent),
                this.GetSearchResults(PlaceType.Country),
                this.GetSearchResults(PlaceType.L1),
                this.GetSearchResults(PlaceType.L2),
                this.GetSearchResults(PlaceType.L3),
                this.GetSearchResults(PlaceType.L4),
                this.GetSearchResults(PlaceType.Beach, 2));

            return results;
        }

        private IEnumerable<SearchResult> GetSearchResults(
            PlaceType type, int take = 1)
        {
            var item = this.postingsItemByType[type.ToString()];
            var results = item.PostingsByToken
                .Select(i => new SearchResult
                {
                    Id = item.Id,
                    Name = i.Key,
                    Type = type.ToString(),
                    BeachesCount = i.Value.Count
                })
                .OrderByDescending(r => r.BeachesCount)
                .Take(take)
                .ToList();

            return results;
        }
    }
}
