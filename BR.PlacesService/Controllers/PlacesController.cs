using BR.Core.Abstractions;
using BR.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.PlacesService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly INoSqlRepository<Place> repo;
        private readonly ILogger<PlacesController> logger;

        public PlacesController(INoSqlRepository<Place> repo, ILogger<PlacesController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{id}/next")]
        public async Task<IActionResult> GetNextAsync(string id)
        {
            try
            {
                var place = await this.repo.GetAsync(id);
                var children = place.Children;

                return Ok(children);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Could not get children of {id}.");

                return BadRequest();
            }
        }
    }
}
