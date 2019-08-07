using BeachRankings.Api.Abstractions;
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
        [Route("places")]
        public async Task<IActionResult> SearchPlacesAsync(string prefix)
        {
            return Ok(await this.searchService.SearchPlacesAsync(prefix));
        }

        [HttpGet]
        [Route("beaches")]
        public async Task<IActionResult> SearchBeachesAsync([FromQuery]BeachQueryModel model)
        {
            return Ok(await this.searchService.SearchBeachesAsync(model));
        }
    }
}
