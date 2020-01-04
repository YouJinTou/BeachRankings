using AutoMapper;
using BR.Core.Abstractions;
using BR.Core.Models;
using BR.Core.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Controllers
{
    [Route("api/v1/[controller]")]
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBeachAsync(string id)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(id, "Missing beach ID.");

                this.logger.LogInformation($"Getting beach {id}.");

                var beach = await this.service.GetBeachAsync(id);
                var beachModel = this.mapper.Map<GetBeachModel>(beach);

                return Ok(beachModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Getting beach {id} failed.");

                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("many")]
        public async Task<IActionResult> GetBeachesAsync([FromBody]IEnumerable<string> ids)
        {
            try
            {
                Validator.ThrowIfNull(ids, "Missing beach IDs.");

                this.logger.LogInformation($"Getting beaches {string.Join(" ", ids)}.");

                var beaches = await this.service.GetBeachesAsync(ids);
                var models = this.mapper.Map<IEnumerable<GetBeachModel>>(beaches);

                return Ok(models);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Getting beach {string.Join(" ", ids)} failed.");

                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBeachAsync([FromBody]CreateBeachModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing beach data.");

                this.logger.LogInformation($"Creating beach {model.Name}.");

                var beach = await this.service.CreateBeachAsync(model);
                var beachModel = this.mapper.Map<GetBeachModel>(beach);

                return Created(beachModel.Id.ToString(), beachModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Creating beach {model?.Name} failed.");

                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ModifyBeachAsync([FromBody]ModifyBeachModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing beach data.");

                this.logger.LogInformation($"Creating beach {model.Name}.");

                var beach = await this.service.ModifyBeachAsync(model);
                var beachModel = this.mapper.Map<GetBeachModel>(beach);

                return Created(beachModel.Id.ToString(), beachModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Creating beach {model?.Name} failed.");

                return BadRequest(ex);
            }
        }
    }
}
