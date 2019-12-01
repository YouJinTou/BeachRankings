using AutoMapper;
using BR.Core.Tools;
using BR.Iam.Abstractions;
using BR.Iam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.Iam.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService service;
        private readonly IMapper mapper;
        private readonly ILogger<UsersController> logger;

        public UsersController(
            IUsersService service, IMapper mapper, ILogger<UsersController> logger)
        {
            this.service = service;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(id);

                this.logger.LogInformation($"Getting user {id}.");

                var user = await this.service.GetUserAsync(id);
                var userModel = this.mapper.Map<GetUserModel>(user);

                return Ok(userModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Getting user {id} failed.");

                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserModel model)
        {
            try
            {
                this.logger.LogInformation($"Creating user {model.Username}.");

                var user = await this.service.CreateUserAsync(model);
                var userModel = this.mapper.Map<GetUserModel>(user);

                return new CreatedResult(userModel.Id.ToString(), userModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Creating user {model.Username} failed.");

                return BadRequest(ex);
            }
        }
    }
}
