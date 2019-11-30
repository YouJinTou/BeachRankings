using AutoMapper;
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
        private readonly IMapper mapper;
        private readonly ILogger<UsersController> logger;

        public UsersController(
            IUsersService service, IMapper mapper, ILogger<UsersController> logger)
        {
            this.service = service;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserModel model)
        {
            var user = await this.service.CreateUserAsync(model);
            var userModel = this.mapper.Map<GetUserModel>(user);

            return new CreatedResult(userModel.Id.ToString(), userModel);
        }
    }
}
