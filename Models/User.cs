namespace BeachRankings.Models
{
    using BeachRankings.Models.Enums;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class User : IdentityUser
    {
        private const int MaxLevel = 7;
        private readonly int[] LevelThresholds = 
            new int[MaxLevel] { 300, 700, 1200, 1800, 2500, 3300, 4200 };

        private ICollection<Review> reviews;
        private ICollection<UpvotedReview> upvotedReviews;
        private ICollection<BeachImage> images;
        private ICollection<Watchlist> watchlists;
        private ClaimsIdentity identity;

        public User()
        {
            this.Level = 1;
            this.Badge = UserBadge.None;
        }

        public string AvatarPath { get; set; }

        public int Points { get; set; }

        public int Level { get; protected set; }

        public int ThanksReceived { get; set; }

        public bool IsBlogger { get; set; }

        public UserBadge Badge { get; set; }

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

        public virtual ICollection<UpvotedReview> UpvotedReviews
        {
            get
            {
                return this.upvotedReviews ?? (this.upvotedReviews = new HashSet<UpvotedReview>());
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

        public virtual ICollection<Watchlist> Watchlists
        {
            get
            {
                return this.watchlists ?? (this.watchlists = new HashSet<Watchlist>());
            }
            protected set
            {
                this.watchlists = value;
            }
        }

        public virtual Blog Blog { get; set; }

        public int CurrentLevelThreshold => this.LevelThresholds[this.Level - 1];

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

        public IDictionary<string, int> GetTopBeachesByCountry(int top)
        {
            var beachesByCountry = this.Reviews
                .GroupBy(r => r.Beach.Country.Name)
                .Select(g => new { Country = g.Key, BeachCount = g.Count() })
                .OrderByDescending(g => g.BeachCount)
                .Take(top)
                .ToDictionary(kvp => kvp.Country, kvp => kvp.BeachCount);

            return beachesByCountry;
        }

        public void RecalculateLevel(ICollection<ScoreWeight> weights)
        {
            var countryPoints = this.GetVisitedCountriesCount() * weights.First(w => w.Name == "Country").Value;
            var beachPoints = this.Reviews.Count * weights.First(w => w.Name == "Beach").Value;
            var thankedPoints = this.ThanksReceived * weights.First(w => w.Name == "Thanked").Value;
            var blogPostPoints = (this.Blog == null) ? 0.0 : 
                this.Blog.BlogArticles.Count * weights.First(w => w.Name == "BlogPost").Value;
            var crqMultiplierValue = weights.First(w => w.Name == "CRQ").Value;
            var crqMultiplier = (this.Badge == UserBadge.None || crqMultiplierValue == 0.0) ? 1.0 : crqMultiplierValue;
            var totalPoints = (int)((countryPoints + beachPoints + thankedPoints + blogPostPoints) * crqMultiplier);
            this.Points = totalPoints;

            for (int index = this.LevelThresholds.Length - 1; index >= 0; index--)
            {                
                if (this.LevelThresholds[index] < this.Points)
                {
                    this.Level = index + 2;

                    return;
                }
            }

            this.Level = 1;
        }
    }
}