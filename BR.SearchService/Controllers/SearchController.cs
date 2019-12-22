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
        private readonly ILogger<SearchController> logger;

        public SearchController(ILogger<SearchController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync(string query)
        {
            try
            {
                return Ok(await Task.FromResult("123"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Running '{query}' failed.");

                return StatusCode(500, ex);
            }
        }
    }
}
