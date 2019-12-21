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
                Validator.ThrowIfNull(model, "Missing auth data.");

                this.logger.LogInformation($"Authenticating user {model.Username}.");

                var result = await this.service.LoginAsync(model);

                return result.IsSuccess ? Ok(result) : (IActionResult)Unauthorized();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Authenticating user {model.Username} failed.");

                return BadRequest(ex);
            }
        }
    }
}
