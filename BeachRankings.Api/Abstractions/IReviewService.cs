using BeachRankings.Api.Models.Reviews;
using System.Threading.Tasks;

namespace BeachRankings.Api.Abstractions
{
    public interface IReviewService
    {
        Task AddAsync(AddReviewModel addReviewModel);
    }
}
