using AutoMapper;
using BeachRankings.Api.Abstractions;
using BeachRankings.Api.Models.Reviews;
using BeachRankings.Core.Abstractions;
using BeachRankings.Core.Models;
using System.Threading.Tasks;

namespace BeachRankings.Api.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Review> reviews;

        public ReviewService(IMapper mapper, IRepository<Review> reviews)
        {
            this.mapper = mapper;
            this.reviews = reviews;
        }

        public async Task AddAsync(AddReviewModel addReviewModel)
        {
            var review = this.mapper.Map<Review>(addReviewModel);

            await this.reviews.AddAsync(review);
        }
    }
}
