using BR.SearchService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                var results = new List<SearchResult>
                {
                    new SearchResult
                    {
                        Id = "7vHSFLZd5R8g6iW",
                        Label = "Rubin Beach"
                    }
                };

                return Ok(await Task.FromResult(results));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Running '{query}' failed.");

                return StatusCode(500, ex);
            }
        }
    }
}
