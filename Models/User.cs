namespace BeachRankings.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class User : IdentityUser
    {
        private ICollection<Review> reviews;
        private ICollection<Review> upvotedReviews;
        private ICollection<BeachImage> images;
        private ClaimsIdentity identity;

        public User()
        {
        }

        public string AvatarPath { get; set; }

        public virtual ICollection<Review> Reviews
        {
            get
            {
                return this.reviews ?? (this.reviews = new HashSet<Review>());
            }
        }

        public virtual ICollection<Review> UpvotedReviews
        {
            get
            {
                return this.upvotedReviews ?? (this.upvotedReviews = new HashSet<Review>());
            }
        }

        public virtual ICollection<BeachImage> Images
        {
            get
            {
                return this.images ?? (this.images = new HashSet<BeachImage>());
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            this.identity = userIdentity;

            return userIdentity;
        }

        public bool CanRateBeach(int id)
        {
            return !this.Reviews.Any(r => r.BeachId == id);
        }

        public int GetVisitedCountriesCount()
        {
            var countriesVisited = this.Reviews.GroupBy(r => r.Beach.Country.Name).Count();

            return countriesVisited;
        }
    }
}