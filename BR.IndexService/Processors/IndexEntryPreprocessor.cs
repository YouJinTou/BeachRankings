using BR.Core.Extensions;
using BR.Core.Models;
using BR.Core.Tools;
using BR.IndexService.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BR.IndexService.Processors
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

        public IEnumerable<IndexEntry> PreprocessToken(IndexToken token, params string[] ids)
        {
            var entries = new HashSet<IndexEntry>(new IndexEntryEqualityComparer());

            if (Validator.AllNull(token))
            {
                return entries;
            }

            var latinized = string.Join(
                "", token.Token.Select(
                    c => this.latinizer.ContainsKey(c) ? this.latinizer[c] : c));
            var lowered = latinized.ToLower();

            foreach (var item in lowered.Split(new[] { ' ', '-', '.', ',', '_', '–' }))
            {
                var normalized = Regex.Replace(item, $"[^a-z]", string.Empty);

                if (string.IsNullOrWhiteSpace(normalized) || normalized.Length == 1)
                {
                    continue;
                }

                entries.Add(new IndexEntry
                {
                    Bucket = normalized.AsBucket(),
                    Token = normalized,
                    Type = token.Type.ToString(),
                    Postings = ids
                });
            }

            return entries;
        }

        public IEnumerable<IndexEntry> PreprocessTokens(IEnumerable<IndexToken> tokens)
        {
            var allEntries = new HashSet<IndexEntry>(new IndexEntryEqualityComparer());

            foreach (var token in tokens.NewIfNull())
            {
                foreach (var entry in this.PreprocessToken(token))
                {
                    allEntries.Add(entry);
                }
            }

            return allEntries;
        }
    }
}
