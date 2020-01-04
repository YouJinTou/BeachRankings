using BR.Core.Models;
using System.Collections.Generic;

namespace BR.Core.Abstractions
{
    internal interface IQueryResultsParser
    {
        IEnumerable<SearchResult> ParseQueryResults(IEnumerable<IndexEntry> entries);
    }
}
