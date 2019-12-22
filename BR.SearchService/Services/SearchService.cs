using BR.Core.Abstractions;
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
        private readonly IEventStore store;
        private readonly ILogger<SearchService> logger;

        public SearchService(IEventStore store, ILogger<SearchService> logger)
        {
            this.store = store;
            this.logger = logger;
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(string query)
        {
            try
            {
                await Task.Delay(0);

                return new List<SearchResult>
                {
                    new SearchResult
                    {
                        Id = "7vHSFLZd5R8g6iW",
                        Label = "Rubin Beach"
                    }
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to search for {query}.");

                throw;
            }
        }
    }
}
