using BR.SearchService.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.SearchService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService service;
        private readonly ILogger<SearchController> logger;

        public SearchController(ISearchService service, ILogger<SearchController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync(string query)
        {
            try
            {
                var results = await this.service.SearchAsync(query);

                return Ok(results);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Running '{query}' failed.");

                return StatusCode(500, ex);
            }
        }
    }
}
