namespace BeachRankings.App.Utils.Extensions
{
    using System.Security.Claims;
    using System.Security.Principal;

    public static class IdentityExtensions
    {
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