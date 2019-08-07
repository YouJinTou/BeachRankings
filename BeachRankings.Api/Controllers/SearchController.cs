using BeachRankings.Api.Abstractions;
using BeachRankings.Core;
using BeachRankings.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeachRankings.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        [Route("beaches")]
        public async Task<IActionResult> SearchAsync(
            string pf = null,
            string ct = null,
            string cy = null,
            string l1 = null,
            string l2 = null,
            string l3 = null,
            string l4 = null,
            string wb = null,
            string orderBy = nameof(Beach.Score),
            string orderDirection = Constants.View.Descending)
        {
            return Ok(await this.searchService.FindBeachesAsync(
                pf, ct, cy, l1, l2, l3, l4, wb, orderBy, orderDirection));
        }
    }
}
