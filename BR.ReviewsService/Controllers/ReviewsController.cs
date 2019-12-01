﻿using AutoMapper;
using BR.Core.Tools;
using BR.ReviewsService.Abstractions;
using BR.ReviewsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.ReviewsService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsService service;
        private readonly IMapper mapper;
        private readonly ILogger<ReviewsController> logger;

        public ReviewsController(
            IReviewsService service, IMapper mapper, ILogger<ReviewsController> logger)
        {
            this.service = service;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetReviewAsync(string id)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(id, "Missing review ID.");

                this.logger.LogInformation($"Getting review {id}.");

                var review = await this.service.GetReviewAsync(id);
                var reviewModel = this.mapper.Map<GetReviewModel>(review);

                return Ok(reviewModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Getting review {id} failed.");

                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReviewAsync([FromBody]CreateReviewModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing review data.");

                this.logger.LogInformation($"Creating review for {model.BeachId}.");

                var review = await this.service.CreateReviewAsync(model);
                var reviewModel = this.mapper.Map<GetReviewModel>(review);

                return Created(reviewModel.Id.ToString(), reviewModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Creating review for {model.BeachId} failed.");

                return BadRequest(ex);
            }
        }
    }
}
