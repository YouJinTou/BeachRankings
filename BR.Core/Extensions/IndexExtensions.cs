using BR.Core.Models;
using BR.Core.Tools;
using System.Collections.Generic;
using System.Linq;

namespace BR.Core.Extensions
{
    public static class IndexExtensions
    {
        public static IEnumerable<IndexEntry> Group(this IEnumerable<IndexEntry> entries)
        {
            var groupedEntries = new Dictionary<string, IndexEntry>();

            foreach (var entry in entries)
            {
                if (groupedEntries.ContainsKey(entry.ToString()))
                {
                    var currentEntry = groupedEntries[entry.ToString()];
                    currentEntry.Postings = Collection.Combine<IndexPosting>(
                        currentEntry.Postings, entry.Postings).Group();
                }
                else
                {
                    groupedEntries[entry.ToString()] = entry;
                }
            }

            return groupedEntries.Values;
        }

        public static IEnumerable<IndexPosting> Group(this IEnumerable<IndexPosting> postings)
        {
            var groupedPostings = new Dictionary<string, List<IndexBeach>>();

            foreach (var posting in postings)
            {
                if (groupedPostings.ContainsKey(posting.ToString()))
                {
                    groupedPostings[posting.ToString()] = new HashSet<IndexBeach>(
                        Collection.Combine<IndexBeach>(
                            posting.Beaches, groupedPostings[posting.ToString()]),
                        new IndexBeachEqualityComparer()).ToList();
                }
                else
                {
                    groupedPostings[posting.ToString()] = posting.Beaches.ToList();
                }
            }

            var result = groupedPostings.Select(p => new IndexPosting
            {
                Id = p.Key.Split('_')[0],
                Place = p.Key.Split('_')[1],
                Type = p.Key.Split('_')[2],
                Beaches = p.Value
            }).ToList();

            return result;
        }
    }
}
