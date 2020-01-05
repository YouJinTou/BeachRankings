using BR.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IReviewsService
    {
        Task<Review> GetReviewAsync(string id);

        Task<IEnumerable<Review>> GetBeachReviewsAsync(string id);

        Task<Review> CreateReviewAsync(CreateReviewModel model);

        Task<Review> ModifyReviewAsync(ModifyReviewModel model);
    }
}
