using BeachRankings.Api.Models.Beaches;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeachRankings.Api.Abstractions
{
    public interface ISearchService
    {
        Task<SearchViewModel> SearchPlacesAsync(string prefix);

        Task<IEnumerable<BeachDbResultModel>> SearchBeachesAsync(BeachQueryModel model);
    }
}
