using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Models;
using BR.Core.Tools;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BR.Core.Processors
{
    public class IndexEntryPreprocessor : IIndexEntryPreprocessor
    {
        public IEnumerable<IndexEntry> PreprocessToken(IndexToken token, IndexBeach beach)
        {
            var entries = new List<IndexEntry>();

            if (Validator.AllNull(token))
            {
                return entries;
            }

            var lowered = token.Token.Latinize().ToLower();

            foreach (var item in lowered.Split(new[] { ' ', '-', '.', ',', '_', '–' }))
            {
                var normalized = Regex.Replace(item, $"[^a-z]", string.Empty);

                if (string.IsNullOrWhiteSpace(normalized) || normalized.Length == 1)
                {
                    continue;
                }

                var key = new IndexKey(normalized);
                var entry = new IndexEntry
                {
                    Bucket = key.Bucket,
                    Token = key.Token
                };
                var postings = new List<IndexPosting>
                {
                    new IndexPosting
                    {
                        Id = token.Type == PlaceType.Beach ? beach.Id : entry.ToString(),
                        Type = token.Type.ToString(),
                        Beaches = Collection.Combine<IndexBeach>(beach),
                        Place = token.Token
                    }
                };
                entry.Postings = postings;

                entries.Add(entry);
            }

            return entries;
        }
    }
}
