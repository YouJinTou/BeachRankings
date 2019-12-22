using BR.SearchService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.SearchService.Abstractions
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResult>> SearchAsync(string query); 
    }
}
