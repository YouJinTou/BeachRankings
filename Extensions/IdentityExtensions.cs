namespace BeachRankings.Extensions
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class IdentityExtensions
    {
        public static bool CanRestructure(this IIdentity identity, string authorId)
        {
            return IsAdmin(identity);
        }

        public static bool CanEditReview(this IIdentity identity, string authorId)
        {
            return IsAdmin(identity) || AuthorIdMatches(identity, authorId);
        }

        public static bool CanVoteForReview(this IIdentity identity, string authorId)
        {
            var claimsIdentity = (ClaimsIdentity)identity;

            if (claimsIdentity == null)
            {
                return false;
            }
            var userIdClaim = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return false;
            }

            var userId = userIdClaim.Value;
            var userCanVote = (userId != authorId);

            return userCanVote;
        }

        public static bool ReviewAlreadyUpvoted(this IIdentity identity, int reviewId, ICollection<int> reviewIds)
        {
            return reviewIds.Contains(reviewId);
        }

        public static bool CanEditBeach(this IIdentity identity, string creatorId, int reviewsCount)
        {
            return IsAdmin(identity) || (AuthorIdMatches(identity, creatorId) && reviewsCount == 0);
        }

        public static bool CanDeleteBeachImage(this IIdentity identity, string authorId)
        {
            return IsAdmin(identity) || AuthorIdMatches(identity, authorId);
        }

        public static bool CanEditWatchlist(this IIdentity identity, string authorId)
        {
            return AuthorIdMatches(identity, authorId);
        }

        public static bool IsAdmin(this IIdentity identity)
        {
            var claimsIdentity = (ClaimsIdentity)identity;

            if (claimsIdentity == null)
            {
                return false;
            }

            var userRoleClaim = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.Role);

            if (userRoleClaim == null)
            {
                return false;
            }

            var userRole = userRoleClaim.Value;

            return userRole == "Admin";
        }

        private static bool AuthorIdMatches(IIdentity identity, string authorId)
        {
            var claimsIdentity = (ClaimsIdentity)identity;

            if (claimsIdentity == null)
            {
                return false;
            }

            var userRoleClaim = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.Role);

            if (userRoleClaim == null)
            {
                return false;
            }

            var userIdClaim = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return false;
            }

            var userId = userIdClaim.Value;
            var canEdit = (userId == authorId);

            return canEdit;
        }
    }
}