using BR.Core.Models;
using BR.SearchService.Models;
using System.Collections.Generic;

namespace BR.SearchService.Abstractions
{
    internal interface IQueryResultsParser
    {
        IEnumerable<SearchResult> ParseQueryResults(IEnumerable<IndexEntry> entries);
    }
}
