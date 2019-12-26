using BR.Core.Cloud.Aws;
using BR.Core.Models;
using BR.IndexService.Processors;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Seed
{
    public static class BeachesSeed
    {
        public static async Task SeedBeachesAsync()
        {
            var db = new DynamoRepository<IndexEntry>("Index");
            using var reader = new StreamReader("beaches.csv");
            var entryPreprocessor = new IndexEntryPreprocessor();
            var allIndices = new HashSet<IndexEntry>(new IndexEntryEqualityComparer());
            var line = string.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                var tokens = line.Split('\t');
                var indices = GetBeachIndices(tokens, entryPreprocessor).ToList();

                indices.ForEach(i => allIndices.Add(i));
            }

            await db.AddManyAsync(allIndices);
        }

        private static IEnumerable<IndexEntry> GetBeachIndices(
            string[] tokens, IndexEntryPreprocessor preprocessor)
        {
            var name = tokens[0];
            var entries = preprocessor.PreprocessToken(name);

            return entries;
        }
    }
}
