using BeachRankings.Core;
using BeachRankings.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeachRankings.Api.Abstractions
{
    public interface ISearchService
    {
        Task<IEnumerable<BeachDbResultModel>> FindBeachesAsync(
            string pf = null,
            string ct = null,
            string cy = null,
            string l1 = null,
            string l2 = null,
            string l3 = null,
            string l4 = null,
            string wb = null,
            string orderBy = nameof(Beach.Score),
            string orderDirection = Constants.View.Descending);
    }
}
