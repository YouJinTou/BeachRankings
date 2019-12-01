using AutoMapper;
using BR.BeachesService.Abstractions;
using BR.BeachesService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.BeachesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeachesController : ControllerBase
    {
        private readonly IBeachesService service;
        private readonly IMapper mapper;
        private readonly ILogger<BeachesController> logger;

        public BeachesController(
            IBeachesService service, IMapper mapper, ILogger<BeachesController> logger)
        {
            this.service = service;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBeachAsync([FromBody]CreateBeachModel model)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return BadRequest(); // TODO
                }

                this.logger.LogInformation($"Creating beach {model.Name}.");

                var beach = await this.service.CreateBeachAsync(model);
                var beachModel = this.mapper.Map<GetBeachModel>(beach);

                return Created(beachModel.Id.ToString(), beachModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Creating user {model.Name} failed.");

                return BadRequest(ex);
            }
        }
    }
}
