using AutoMapper;
using BR.Core.Abstractions;
using BR.Core.Caching;
using BR.Core.Exceptions;
using BR.Core.Models;
using BR.Core.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.Core.Controllers
{
    [ApiController]
    [Route("api/v1/iam/[controller]")]
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
                Validator.ThrowIfNullOrWhiteSpace(id, "Missing user ID.");

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
                try
                {
                    this.logger.LogInformation($"Creating user {model?.Username}.");

                    Validator.ThrowIfNull(model, "Missing user data.");

                    var user = await this.service.CreateUserAsync(model);
                    var resultModel = this.mapper.Map<CreateUserResult>(user);

                    return new CreatedResult(resultModel.Id, resultModel);
                }
                catch (UserCreationFailedException ucfe)
                {
                    var result = new CreateUserResult
                    {
                        IsSuccess = false,
                        Message = ucfe.Message
                    };

                    return UnprocessableEntity(result);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Creating user {model.Username} failed.");

                var result = new CreateUserResult
                {
                    IsSuccess = false,
                    Message = "We could not process the request."
                };

                return StatusCode(500, result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ModifyUserAsync([FromBody]ModifyUserModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing user data.");

                this.logger.LogInformation($"Creating user {model.Id}.");

                var user = await this.service.ModifyUserAsync(model);
                var userModel = this.mapper.Map<GetUserModel>(user);

                return Ok(userModel);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Creating user {model.Id} failed.");

                return BadRequest(ex);
            }
        }
    }
}
