using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BR.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> PerformHealthCheckAsync()
        {
            await Task.CompletedTask;

            return Ok();
        }
    }
}
