using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.SearchService.Abstractions;
using BR.SearchService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.SearchService.Services
{
    internal class SearchService : ISearchService
    {
        private readonly INoSqlRepository<IndexEntry> repo;
        private readonly IQueryResultsParser parser;
        private readonly ILogger<SearchService> logger;

        public SearchService(
            INoSqlRepository<IndexEntry> repo, 
            IQueryResultsParser parser, 
            ILogger<SearchService> logger)
        {
            this.repo = repo;
            this.parser = parser;
            this.logger = logger;
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query) || query.Length == 1)
                {
                    return new List<SearchResult>();
                }

                var allEntries = new List<IndexEntry>();

                foreach (var token in query.Split(" "))
                {
                    var key = new IndexKey(token);
                    var entries = await this.repo.GetManyBeginsWithAsync(
                    "Bucket", key.Bucket, "Token", key.Token);

                    allEntries.AddRange(entries);
                }

                var results = this.parser.ParseQueryResults(allEntries).ToList();

                return results;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to search for {query}.");

                throw;
            }
        }

        public async Task<PlaceSearchResult> SearchPlaceAsync(string id, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
                {
                    return new PlaceSearchResult();
                }

                var key = new IndexKey(id);
                var entry = await this.repo.GetAsync(key.Bucket, key.Token);
                var place = entry.Postings.Single(p => p.Place == name);
                var result = new PlaceSearchResult
                {
                    Beaches = place.Beaches
                };

                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to search for {id}/{name}.");

                throw;
            }
        }
    }
}
