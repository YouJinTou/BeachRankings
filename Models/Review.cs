namespace BeachRankings.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        private const int BeachCriteriaCount = 15;

        private ICollection<BlogArticle> blogArticles;

        protected Review()
        {
        }

        public Review(
            int beachId,
            string content,
            double? sandQuality,
            double? beachCleanliness,
            double? beautifulScenery,
            double? crowdFree,
            double? infrastructure,
            double? waterVisibility,
            double? litterFree,
            double? feetFriendlyBottom,
            double? seaLifeDiversity,
            double? coralReef,
            double? snorkeling,
            double? kayaking,
            double? walking,
            double? camping,
            double? longTermStay
            )
        {
            this.BeachId = beachId;
            this.PostedOn = DateTime.Now;
            this.Content = content;

            this.SandQuality = sandQuality;
            this.BeachCleanliness = beachCleanliness;
            this.BeautifulScenery = beautifulScenery;
            this.CrowdFree = crowdFree;
            this.Infrastructure = infrastructure;

            this.WaterVisibility = waterVisibility;
            this.LitterFree = litterFree;
            this.FeetFriendlyBottom = feetFriendlyBottom;
            this.SeaLifeDiversity = seaLifeDiversity;
            this.CoralReef = coralReef;

            this.Snorkeling = snorkeling;
            this.Kayaking = kayaking;
            this.Walking = walking;
            this.Camping = camping;
            this.LongTermStay = longTermStay;

            this.UpdateScores();
        }

        [Key]
        public int Id { get; set; }

        public virtual User Author { get; protected set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public int BeachId { get; protected set; }

        public virtual Beach Beach { get; protected set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }
        
        public int Upvotes { get; set; }

        public int Points { get; set; }

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

        [Range(0, 10)]
        public double? TotalScore { get; protected set; }

        #region Beachline

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; protected set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; protected set; }

        [Display(Name = "Beautiful scenery")]
        public double? BeautifulScenery { get; protected set; }

        [Display(Name = "Crowd-free")]
        public double? CrowdFree { get; protected set; }

        [Display(Name = "Infrastructure")]
        public double? Infrastructure { get; protected set; }

        #endregion

        #region Water

        [Display(Name = "Underwater visibility")]
        public double? WaterVisibility { get; protected set; }

        [Display(Name = "Litter-free water")]
        public double? LitterFree { get; protected set; }

        [Display(Name = "Feet-friendly bottom")]
        public double? FeetFriendlyBottom { get; protected set; }

        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; protected set; }

        [Display(Name = "Coral reef wow factor")]
        public double? CoralReef { get; protected set; }

        #endregion

        #region Activities

        [Display(Name = "Snorkeling")]
        public double? Snorkeling { get; protected set; }

        [Display(Name = "Kayaking")]
        public double? Kayaking { get; protected set; }

        [Display(Name = "Taking a walk")]
        public double? Walking { get; protected set; }

        [Display(Name = "Camping")]
        public double? Camping { get; protected set; }

        [Display(Name = "Long-term stay")]
        public double? LongTermStay { get; protected set; }

        #endregion

        public void UpdateScores()
        {
            double score = 0;
            int nullCount = 0; // Count of criteria NOT voted for

            score += (this.SandQuality ?? 0);
            nullCount += ((this.SandQuality == null) ? 1 : 0);

            score += (this.BeachCleanliness ?? 0);
            nullCount += ((this.BeachCleanliness == null) ? 1 : 0);

            score += (this.BeautifulScenery ?? 0);
            nullCount += ((this.BeautifulScenery == null) ? 1 : 0);

            score += (this.CrowdFree ?? 0);
            nullCount += ((this.CrowdFree == null) ? 1 : 0);

            score += (this.Infrastructure ?? 0);
            nullCount += ((this.Infrastructure == null) ? 1 : 0);

            score += (this.WaterVisibility ?? 0);
            nullCount += ((this.WaterVisibility == null) ? 1 : 0);

            score += (this.LitterFree ?? 0);
            nullCount += ((this.LitterFree == null) ? 1 : 0);

            score += (this.FeetFriendlyBottom ?? 0);
            nullCount += ((this.FeetFriendlyBottom == null) ? 1 : 0);

            score += (this.SeaLifeDiversity ?? 0);
            nullCount += ((this.SeaLifeDiversity == null) ? 1 : 0);

            score += (this.CoralReef ?? 0);
            nullCount += ((this.CoralReef == null) ? 1 : 0);

            score += (this.Snorkeling ?? 0);
            nullCount += ((this.Snorkeling == null) ? 1 : 0);
            
            score += (this.Kayaking ?? 0);
            nullCount += ((this.Kayaking == null) ? 1 : 0);

            score += (this.Walking ?? 0);
            nullCount += ((this.Walking == null) ? 1 : 0);

            score += (this.Camping ?? 0);
            nullCount += ((this.Camping == null) ? 1 : 0);

            score += (this.LongTermStay ?? 0);
            nullCount += ((this.LongTermStay == null) ? 1 : 0);

            double? result = null;

            if (nullCount != BeachCriteriaCount)
            {
                result = Math.Round((double)(score / (BeachCriteriaCount - nullCount)), 1);
            }

            this.TotalScore = result;
            this.Points = (BeachCriteriaCount - nullCount);
        }
    }
}