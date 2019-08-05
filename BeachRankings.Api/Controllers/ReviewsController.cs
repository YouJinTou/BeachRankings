using BeachRankings.Api.Abstractions;
using BeachRankings.Api.Models.Reviews;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeachRankings.Api.Controllers
{
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody]AddReviewModel review)
        {
            await this.reviewService.AddAsync(review);

            return Ok();
        }
    }
}
