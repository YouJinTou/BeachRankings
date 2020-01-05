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
        private readonly IBeachesService beachesService;
        private readonly IReviewsService reviewsService;
        private readonly IMapper mapper;
        private readonly ILogger<BeachesController> logger;

        public BeachesController(
            IBeachesService beachesService, 
            IReviewsService reviewsService,
            IMapper mapper, 
            ILogger<BeachesController> logger)
        {
            this.beachesService = beachesService;
            this.reviewsService = reviewsService;
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

                var beach = await this.beachesService.GetBeachAsync(id);
                var reviews = await this.reviewsService.GetBeachReviewsAsync(id);
                var beachModel = this.mapper.Map<GetBeachModel>(beach);
                beachModel.Reviews = this.mapper.Map<IEnumerable<GetReviewModel>>(reviews);

                return Ok(beachModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Getting beach {id} failed.");

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

                var beach = await this.beachesService.CreateBeachAsync(model);
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

                var beach = await this.beachesService.ModifyBeachAsync(model);
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
