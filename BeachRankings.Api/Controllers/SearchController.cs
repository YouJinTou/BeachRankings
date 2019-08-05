using BeachRankings.Api.Abstractions;
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
        public async Task<IActionResult> SearchAsync(string prefix)
        {
            return Ok(await this.searchService.FindBeachesAsync(prefix));
        }
    }
}
