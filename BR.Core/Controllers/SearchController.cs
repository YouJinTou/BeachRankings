﻿using BR.Core.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.Core.Controllers
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

        [HttpGet]
        [Route("places")]
        public async Task<IActionResult> SearchPlaceAsync(string id, string name, string type)
        {
            try
            {
                var result = await this.service.SearchPlaceAsync(id, name, type);

                return Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Running '{id}/{name}' failed.");

                return StatusCode(500, ex);
            }
        }
    }
}
