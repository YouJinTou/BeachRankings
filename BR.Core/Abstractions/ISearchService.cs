using BR.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResult>> SearchAsync(string query);

        Task<PlaceSearchResult> SearchPlaceAsync(string id, string name);
    }
}
