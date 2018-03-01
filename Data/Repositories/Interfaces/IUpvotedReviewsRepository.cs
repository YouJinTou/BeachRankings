using BeachRankings.Models;

namespace BeachRankings.Data.Repositories.Interfaces
{
    public interface IUpvotedReviewsRepository : IGenericRepository<UpvotedReview>
    {
        bool UserHasVotedForReview(string userId, int reviewId);

        UpvotedReview GetReviewForUser(string userId, int reviewId);
    }
}
