using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeachRankings.Api.Abstractions
{
    public interface ISearchService
    {
        Task<IEnumerable<BeachDbResultModel>> FindBeachesAsync(string prefix);

        Task<IEnumerable<BeachDbResultModel>> FindBeachesAsync(BeachQueryModel model);
    }
}
