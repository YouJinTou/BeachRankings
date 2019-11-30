using BR.Iam.Abstractions;
using BR.Iam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BR.Iam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController
    {
        private readonly IUsersService service;
        private readonly ILogger<UsersController> logger;

        public UsersController(IUsersService service, ILogger<UsersController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserModel model)
        {
            await this.service.CreateUserAsync(model);

            return new OkResult();
        }
    }
}
