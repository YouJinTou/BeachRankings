using BR.Core.Tools;
using BR.Core.Abstractions;
using BR.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.Core.Controllers
{
    [ApiController]
    [Route("api/v1/iam/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;
        private readonly ILogger<AuthController> logger;

        public AuthController(
            IAuthService service, ILogger<AuthController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing login data.");

                this.logger.LogInformation($"Logging in user {model.Username}.");

                var result = await this.service.LoginAsync(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Logging in user {model.Username} failed.");

                return Ok(new LoginResult());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody]AuthModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing auth data.");

                this.logger.LogInformation($"Authenticating user {model.Username}.");

                var result = await this.service.AuthenticateAsync(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Authenticating user {model.Username} failed.");

                return Ok(new AuthModel());
            }
        }
    }
}
