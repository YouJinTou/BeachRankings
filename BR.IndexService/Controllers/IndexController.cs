using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.IndexService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ILogger<IndexController> logger;

        public IndexController(ILogger<IndexController> logger)
        {
            this.logger = logger;
        }
       
        [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to index.");

                throw;
            }
        }
    }
}
