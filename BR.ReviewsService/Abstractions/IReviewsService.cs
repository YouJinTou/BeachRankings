using BR.ReviewsService.Models;
using System.Threading.Tasks;

namespace BR.ReviewsService.Abstractions
{
    public interface IReviewsService
    {
        Task<Review> GetReviewAsync(string id);

        Task<Review> CreateReviewAsync(CreateReviewModel model);
    }
}
