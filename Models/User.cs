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
        private readonly int[] LevelThresholds = new int[] { 10, 20, 50, 100, 200 };

        private ICollection<Review> reviews;
        private ICollection<Review> upvotedReviews;
        private ICollection<BeachImage> images;
        private ICollection<Blog> blogs;
        private ClaimsIdentity identity;

        public User()
        {
            this.Level = 1;
        }

        public string AvatarPath { get; set; }

        public int Level { get; private set; }

        public bool IsBlogger { get; set; }

        public virtual ICollection<Review> Reviews
        {
            get
            {
                return this.reviews ?? (this.reviews = new HashSet<Review>());
            }
            protected set
            {
                this.reviews = value;
            }
        }

        public virtual ICollection<Review> UpvotedReviews
        {
            get
            {
                return this.upvotedReviews ?? (this.upvotedReviews = new HashSet<Review>());
            }
            protected set
            {
                this.upvotedReviews = value;
            }
        }

        public virtual ICollection<BeachImage> Images
        {
            get
            {
                return this.images ?? (this.images = new HashSet<BeachImage>());
            }
            protected set
            {
                this.images = value;
            }
        }

        public virtual ICollection<Blog> Blogs
        {
            get
            {
                return this.blogs ?? (this.blogs = new HashSet<Blog>());
            }
            protected set
            {
                this.blogs = value;
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

        public void RecalculateLevel()
        {
            var reviewsCount = this.Reviews.Count;

            for (int i = this.LevelThresholds.Length - 1; i >= 0; i--)
            {                
                if (this.LevelThresholds[i] < reviewsCount)
                {
                    this.Level = i + 1;

                    break;
                }
            }
        }
    }
}