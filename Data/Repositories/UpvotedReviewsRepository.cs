using BeachRankings.Models;
using BeachRankings.Data.Repositories.Interfaces;
using System.Data.Entity;
using System.Linq;

namespace BeachRankings.Data.Repositories
{
    public class UpvotedReviewsRepository : GenericRepository<UpvotedReview>, IUpvotedReviewsRepository
    {
        private DbContext dbContext;
        private IDbSet<UpvotedReview> entitySet;

        public UpvotedReviewsRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.entitySet = dbContext.Set<UpvotedReview>();
        }

        public UpvotedReview GetReviewForUser(string userId, int reviewId)
        {
            return this.entitySet
                .FirstOrDefault(ur => ur.UpvotingUserId == userId && ur.AssociatedReviewId == reviewId);
        }

        public bool UserHasVotedForReview(string userId, int reviewId)
        {
            return this.entitySet
                .Where(ur => ur.UpvotingUserId == userId)
                .Any(ur => ur.AssociatedReviewId == reviewId);
        }
    }
}
