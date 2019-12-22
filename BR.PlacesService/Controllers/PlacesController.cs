using BR.Core.Abstractions;
using BR.Core.Models;
using BR.PlacesService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
        [Route("countries")]
        public async Task<IActionResult> GetCountriesAsync()
        {
            try
            {
                return Ok(await Task.FromResult(Models.Places.Countries));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Could not get countries.");

                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{id}/next")]
        public async Task<IActionResult> GetNextAsync(string id)
        {
            try
            {
                var place = await this.repo.GetAsync(id);
                var children = place?.Children?.Select(c => new GetNextModel
                {
                    Id = c,
                    Name = c.Split('_').Last()
                }).ToList();

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
