using BeachRankings.Api.Abstractions;
using BeachRankings.Api.Models.Beaches;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeachRankings.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeachesController : ControllerBase
    {
        private readonly IBeachService beachService;

        public BeachesController(IBeachService beachService)
        {
            this.beachService = beachService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            return Ok(await this.beachService.GetAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody]AddBeachModel beach)
        {
            await this.beachService.AddAsync(beach);

            return Ok();
        }
    }
}
