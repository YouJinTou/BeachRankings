using BR.Core.Models;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IReviewsService
    {
        Task<Review> GetReviewAsync(string id);

        Task<Review> CreateReviewAsync(CreateReviewModel model);

        Task<Review> ModifyReviewAsync(ModifyReviewModel model);
    }
}
