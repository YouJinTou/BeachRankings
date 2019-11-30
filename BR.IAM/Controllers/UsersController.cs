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
        private readonly ILogger<UsersController> logger;

        public UsersController(ILogger<UsersController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserModel model)
        {
            return new OkResult();
        }
    }
}
