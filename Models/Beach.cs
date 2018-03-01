namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class Beach : IBeachSearchable
    {
        private ICollection<Review> reviews;
        private ICollection<BeachImage> images;
        private ICollection<BlogArticle> blogArticles;
        private ICollection<Watchlist> watchlists;

        public Beach()
        {
            this.AddedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        [Index("IX_BeachUnique", IsUnique = true, Order = 0)]
        public string Name { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }

        [Required]
        public DateTime AddedOn { get; set; }

        public int? ContinentId { get; set; }

        public virtual Continent Continent { get; set; }

        [Required]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        [Index("IX_BeachUnique", IsUnique = true, Order = 1)]
        public int? PrimaryDivisionId { get; set; }

        public virtual PrimaryDivision PrimaryDivision { get; protected set; }

        [Index("IX_BeachUnique", IsUnique = true, Order = 2)]
        public int? SecondaryDivisionId { get; set; }

        public virtual SecondaryDivision SecondaryDivision { get; protected set; }

        [Index("IX_BeachUnique", IsUnique = true, Order = 3)]
        public int? TertiaryDivisionId { get; set; }

        public virtual TertiaryDivision TertiaryDivision { get; protected set; }

        [Index("IX_BeachUnique", IsUnique = true, Order = 4)]
        public int? QuaternaryDivisionId { get; set; }

        public virtual QuaternaryDivision QuaternaryDivision { get; protected set; }

        [Required]
        public int WaterBodyId { get; set; }

        public virtual WaterBody WaterBody { get; protected set; }

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }

        public string Address { get; set; }

        public string Coordinates { get; set; }

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

        public virtual ICollection<BlogArticle> BlogArticles
        {
            get
            {
                return this.blogArticles ?? (this.blogArticles = new HashSet<BlogArticle>());
            }
            protected set
            {
                this.blogArticles = value;
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

        [Range(0, 10)]
        public double? TotalScore { get; set; }

        #region Beachline

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; set; }

        [Display(Name = "Beautiful scenery")]
        public double? BeautifulScenery { get; set; }

        [Display(Name = "Crowd-free")]
        public double? CrowdFree { get; set; }

        [Display(Name = "Infrastructure")]
        public double? Infrastructure { get; set; }

        #endregion

        #region Water

        [Display(Name = "Water visibility")]
        public double? WaterVisibility { get; set; }

        [Display(Name = "Litter-free water")]
        public double? LitterFree { get; set; }

        [Display(Name = "Feet-friendly bottom")]
        public double? FeetFriendlyBottom { get; set; }

        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; set; }

        [Display(Name = "Coral reef wow factor")]
        public double? CoralReef { get; set; }

        #endregion

        #region Activities

        [Display(Name = "Snorkeling")]
        public double? Snorkeling { get; set; }

        [Display(Name = "Kayaking")]
        public double? Kayaking { get; set; }

        [Display(Name = "Taking a walk")]
        public double? Walking { get; set; }

        [Display(Name = "Camping")]
        public double? Camping { get; set; }

        [Display(Name = "Long-term stay")]
        public double? LongTermStay { get; set; }

        #endregion

        public void SetBeachData()
        {
            var interval = "-";
            var primaryDivisionName = (this.PrimaryDivision == null) ? null : this.PrimaryDivision.Name + interval;
            var secondaryDivisionName = (this.SecondaryDivision == null) ? null : this.SecondaryDivision.Name + interval;
            var tertiaryDivisionName = (this.TertiaryDivision == null) ? null : this.TertiaryDivision.Name + interval;
            var quaternaryDivisinoName = (this.QuaternaryDivision == null) ? null : this.QuaternaryDivision.Name + interval;
            this.Address = (
                this.Country.Name + interval +
                primaryDivisionName +
                secondaryDivisionName +
                tertiaryDivisionName +
                quaternaryDivisinoName +
                this.WaterBody.Name)
                .Trim();
            this.ContinentId = this.Country.ContinentId;
        }

        public void UpdateScores()
        {
            var reviews = this.Reviews.AsQueryable().Include(r => r.Author).ToList();
            var scores = reviews.Select(r => new AuthorScore
            {
                Level = r.Author.Level,
                Score = new Score
                {
                    SandQuality = r.SandQuality * r.Author.Level,
                    BeachCleanliness = r.BeachCleanliness * r.Author.Level,
                    BeautifulScenery = r.BeautifulScenery * r.Author.Level,
                    CrowdFree = r.CrowdFree * r.Author.Level,
                    Infrastructure = r.Infrastructure * r.Author.Level,
                    WaterVisibility = r.WaterVisibility * r.Author.Level,
                    LitterFree = r.LitterFree * r.Author.Level,
                    FeetFriendlyBottom = r.FeetFriendlyBottom * r.Author.Level,
                    SeaLifeDiversity = r.SeaLifeDiversity * r.Author.Level,
                    CoralReef = r.CoralReef * r.Author.Level,
                    Snorkeling = r.Snorkeling * r.Author.Level,
                    Kayaking = r.Kayaking * r.Author.Level,
                    Walking = r.Walking * r.Author.Level,
                    Camping = r.Camping * r.Author.Level,
                    LongTermStay = r.LongTermStay * r.Author.Level,
                    TotalScore = r.TotalScore * r.Author.Level
                }
            }).ToList();

            this.SandQuality = this.GetWeightedScore(scores, s => s.SandQuality);
            this.BeachCleanliness = this.GetWeightedScore(scores, s => s.BeachCleanliness);
            this.BeautifulScenery = this.GetWeightedScore(scores, s => s.BeautifulScenery);
            this.CrowdFree = this.GetWeightedScore(scores, s => s.CrowdFree);
            this.Infrastructure = this.GetWeightedScore(scores, s => s.Infrastructure);

            this.WaterVisibility = this.GetWeightedScore(scores, s => s.WaterVisibility);
            this.LitterFree = this.GetWeightedScore(scores, s => s.LitterFree);
            this.FeetFriendlyBottom = this.GetWeightedScore(scores, s => s.FeetFriendlyBottom);
            this.SeaLifeDiversity = this.GetWeightedScore(scores, s => s.SeaLifeDiversity);
            this.CoralReef = this.GetWeightedScore(scores, s => s.CoralReef);

            this.Snorkeling = this.GetWeightedScore(scores, s => (s.Snorkeling));
            this.Kayaking = this.GetWeightedScore(scores, s => s.Kayaking);
            this.Walking = this.GetWeightedScore(scores, s => s.Walking);
            this.Camping = this.GetWeightedScore(scores, s => s.Camping);
            this.LongTermStay = this.GetWeightedScore(scores, s => s.LongTermStay);

            this.TotalScore = this.GetWeightedScore(scores, s => s.TotalScore);
        }

        private double? GetWeightedScore(IEnumerable<AuthorScore> scores, Func<Score, double?> summable)
        {
            var cumulativeWeights = scores.Select(s => summable(s.Score) == null ? 0 : s.Level).Sum();

            if (cumulativeWeights == 0)
            {
                return null;
            }

            var scoresSum = scores.Select(s => summable(s.Score)).Sum(s => s ?? null);

            return scoresSum.HasValue ?
                Math.Round(scoresSum.Value / cumulativeWeights, 1) :
                (double?)null;
        }

        private struct AuthorScore
        {
            public int Level { get; set; }

            public Score Score { get; set; }
        }
    }
}