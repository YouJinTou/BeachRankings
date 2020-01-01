using BR.Core.Models;
using BR.Core.Tools;
using System.Collections.Generic;
using System.Linq;

namespace BR.Seed.Extensions
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
            var groupedPostings = new Dictionary<string, List<string>>();

            foreach (var posting in postings)
            {
                if (groupedPostings.ContainsKey(posting.ToString()))
                {
                    groupedPostings[posting.ToString()] = new HashSet<string>(
                        Collection.Combine<string>(
                            posting.BeachIds, groupedPostings[posting.ToString()])).ToList();
                }
                else
                {
                    groupedPostings[posting.ToString()] = posting.BeachIds.ToList();
                }
            }

            var result = groupedPostings.Select(p => new IndexPosting
            {
                Place = p.Key.Split('|')[0],
                Type = p.Key.Split('|')[1],
                BeachIds = p.Value
            }).ToList();

            return result;
        }
    }
}
