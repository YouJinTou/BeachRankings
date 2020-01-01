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
                if (string.IsNullOrWhiteSpace(query))
                {
                    return new List<SearchResult>();
                }

                var entries = await this.repo.GetManyBeginsWithAsync(
                    "Bucket", query.AsBucket(), "Token", query.ToLower());
                var results = this.parser.ParseQueryResults(entries).ToList();

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
