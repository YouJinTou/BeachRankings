using BR.Core;
using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.SearchService.Abstractions;
using BR.SearchService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.SearchService.Services
{
    internal class SearchService : ISearchService
    {
        private readonly INoSqlRepository<IndexEntry> repo;
        private readonly ILogger<SearchService> logger;

        public SearchService(INoSqlRepository<IndexEntry> repo, ILogger<SearchService> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(string query)
        {
            try
            {
                var results = new List<SearchResult>();

                if (string.IsNullOrWhiteSpace(query))
                {
                    return results;
                }

                var entries = await this.repo.GetManyBeginsWithAsync(
                    "Bucket", query.AsBucket(), "Token", query.ToLower());
                var idsByType = new Dictionary<string, IEnumerable<string>>
                {
                    { PlaceType.Continent.ToString(), new List<string>() },
                    { PlaceType.Country.ToString(), new List<string>() },
                    { PlaceType.L1.ToString(), new List<string>() },
                    { PlaceType.L2.ToString(), new List<string>() },
                    { PlaceType.L3.ToString(), new List<string>() },
                    { PlaceType.L4.ToString(), new List<string>() },
                    { PlaceType.WaterBody.ToString(), new List<string>() },
                    { PlaceType.Beach.ToString(), new List<string>() }
                };

                foreach (var entry in entries)
                {
                    idsByType.Add(entry.ToString(), entry.Postings);

                    foreach (var posting in entry.Postings)
                    {
                        results.Add(new SearchResult
                        {
                            Id = posting,
                            Label = entry.Token
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to search for {query}.");

                throw;
            }
        }
    }
}
