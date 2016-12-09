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
        private readonly int[] LevelThresholds = new int[] { 130, 320, 720, 1600, 3400 };

        private ICollection<Review> reviews;
        private ICollection<Review> upvotedReviews;
        private ICollection<BeachImage> images;
        private ClaimsIdentity identity;

        public User()
        {
            this.Level = 1;
        }

        public string AvatarPath { get; set; }

        public int Points { get; set; }

        public int Level { get; private set; }

        public int ThanksReceived { get; set; }

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

        public virtual Blog Blog { get; set; }

        public string PointsToNextLevel
        {
            get
            {
                var isAtMaxLevel = (this.Level == 6);

                if (isAtMaxLevel)
                {
                    return string.Empty;
                }

                var index = (this.Level - 1);
                var pointsToGo = (this.Points + "/" + this.LevelThresholds[index]);

                return pointsToGo;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
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
            var reviewsPoints = this.Reviews.Sum(r => r.Points);
            var totalPoints = (reviewsCount + reviewsPoints);
            var maxLevelIndex = 4;
            this.Points = (totalPoints > this.LevelThresholds[maxLevelIndex]) ? this.LevelThresholds[maxLevelIndex] : totalPoints;

            for (int index = this.LevelThresholds.Length - 1; index >= 0; index--)
            {                
                if (this.LevelThresholds[index] < this.Points)
                {
                    this.Level = index + 2;

                    return;
                }
            }
        }
    }
}