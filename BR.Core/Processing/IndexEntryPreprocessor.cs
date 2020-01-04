using BR.Core.Models;
using BR.Core.Tools;
using BR.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BR.Core.Processors
{
    public class IndexEntryPreprocessor : IIndexEntryPreprocessor
    {
        private readonly IDictionary<char, char> latinizer = new Dictionary<char, char>
        {
            { 'ă', 'a' },
            { 'á', 'a' },
            { 'ã', 'a' },
            { 'å', 'a' },
            { 'ä', 'a' },
            { 'ā', 'a' },
            { 'â', 'a' },
            { 'à', 'a' },
            { 'ả', 'a' },
            { 'ạ', 'a' },
            { 'ẵ', 'a' },
            { 'ậ', 'a' },
            { 'ć', 'c' },
            { 'č', 'c' },
            { 'ç', 'c' },
            { 'đ', 'd' },
            { 'ð', 'd' },
            { 'ë', 'e' },
            { 'é', 'e' },
            { 'ê', 'e' },
            { 'è', 'e' },
            { 'ė', 'e' },
            { 'ệ', 'e' },
            { 'ế', 'e' },
            { 'ğ', 'g' },
            { 'ï', 'i' },
            { 'í', 'i' },
            { 'ì', 'i' },
            { 'î', 'i' },
            { 'ı', 'i' },
            { 'ĩ', 'i' },
            { 'ị', 'i' },
            { 'ñ', 'n' },
            { 'ó', 'o' },
            { 'ô', 'o' },
            { 'ō', 'o' },
            { 'õ', 'o' },
            { 'ö', 'o' },
            { 'ò', 'o' },
            { 'ồ', 'o' },
            { 'ố', 'o' },
            { 'ø', 'o' },
            { 'ş', 's' },
            { 'š', 's' },
            { 'ț', 't' },
            { 'ü', 'u' },
            { 'ú', 'u' },
            { 'ū', 'u' },
            { 'ũ', 'u' },
            { 'ừ', 'u' },
            { 'ž', 'z' },
        };

        public IEnumerable<IndexEntry> PreprocessToken(IndexToken token, IndexBeach beach)
        {
            var entries = new List<IndexEntry>();

            if (Validator.AllNull(token))
            {
                return entries;
            }

            var latinized = string.Join(
                string.Empty, token.Token.Select(
                    c => this.latinizer.ContainsKey(c) ? this.latinizer[c] : c));
            var lowered = latinized.ToLower();

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
