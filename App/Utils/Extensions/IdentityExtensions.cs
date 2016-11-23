namespace BeachRankings.App.Utils.Extensions
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class IdentityExtensions
    {
        public static bool CanRestructure(this IIdentity identity, string authorId)
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

            return (userRoleClaim.Value == "Admin");
        }

        public static bool CanEditReview(this IIdentity identity, string authorId)
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

            if (userRole == "Admin")
            {
                return true;
            }

            var userIdClaim = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return false;
            }

            var userId = userIdClaim.Value;
            var userCanEdit = (userId == authorId);

            return userCanEdit;
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

            if (userRole == "Admin")
            {
                return true;
            }

            var userIdClaim = claimsIdentity.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return false;
            }

            var userId = userIdClaim.Value;
            var userCanEdit = (userId == creatorId && reviewsCount == 0);

            return userCanEdit;
        }        
    }
}