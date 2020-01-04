﻿using BR.Core.Abstractions;
using BR.Core.Models;
using BR.Core.Tools;
using BR.IndexService.Abstractions;
using BR.IndexService.Processors;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task AddToIndexAsync(IndexData data)
        {
            try
            {
                Validator.ThrowIfNull(data, $"{nameof(IndexData)} empty.");

                await Task.CompletedTask;
//                var entriesToCheck = this.preprocessor.PreprocessTokens(data.Tokens);
//                var updatableEntries = new List<IndexEntry>();

//                foreach (var entry in entriesToCheck)
//                {
//                    try
//                    {
//                        var existingEntry = await this.repo.GetAsync(entry.Bucket, entry.Token);
//#warning: "Here"
//                        //var newPostings = new List<string>(existingEntry.BeachPostings);

//                        //newPostings.AddRange(data.Ids);

//                        //existingEntry.Postings = newPostings;

//                        //updatableEntries.Add(existingEntry);
//                    }
//                    catch (KeyNotFoundException)
//                    {
//                        updatableEntries.Add(entry);
//                    }
//                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to index tokens.");

                throw;
            }
        }
    }
}
