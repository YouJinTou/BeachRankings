using BR.Core.Abstractions;
using BR.Core.Models;
using BR.IndexService.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.IndexService.Services
{
    public class IndexService : IIndexService
    {
        private readonly INoSqlRepository<IndexEntry> repo;
        private readonly IIndexEntryPreprocessor preprocessor;
        private readonly ILogger<IndexService> logger;

        public IndexService(
            INoSqlRepository<IndexEntry> repo, 
            IIndexEntryPreprocessor preprocessor, 
            ILogger<IndexService> logger)
        {
            this.repo = repo;
            this.preprocessor = preprocessor;
            this.logger = logger;
        }

        public async Task AddToIndexAsync(params string[] tokens)
        {
            try
            {
                //var existingEntries = this.repo.GetManyAsync();
                //var entries = this.preprocessor.PreprocessTokens(tokens);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to index tokens.");

                throw;
            }
        }
    }
}
