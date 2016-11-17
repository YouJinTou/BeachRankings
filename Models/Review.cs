namespace BeachRankings.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        private const int BeachCriteriaCount = 15;

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
            double? waterPurity,
            double? wasteFreeSeabed,
            double? feetFriendlyBottom,
            double? seaLifeDiversity,
            double? coralReef,
            double? walking,
            double? snorkeling,
            double? kayaking,
            double? camping,
            double? infrastructure,
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

            this.WaterPurity = waterPurity;
            this.WasteFreeSeabed = wasteFreeSeabed;
            this.FeetFriendlyBottom = feetFriendlyBottom;
            this.SeaLifeDiversity = seaLifeDiversity;
            this.CoralReef = coralReef;

            this.Walking = walking;
            this.Snorkeling = snorkeling;
            this.Kayaking = kayaking;
            this.Camping = camping;

            this.Infrastructure = infrastructure;
            this.LongTermStay = longTermStay;

            this.UpdateTotalScore();
        }

        [Key]
        public int Id { get; set; }

        public virtual User Author { get; protected set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public int BeachId { get; private set; }

        public virtual Beach Beach { get; protected set; }

        [Required]
        public DateTime PostedOn { get; private set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(100, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; private set; }

        public string ArticleLinks { get; set; }

        public int Upvotes { get; set; }

        [Range(0, 10)]
        public double? TotalScore { get; private set; }

        #region Beachline

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; private set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; private set; }

        [Display(Name = "Beautiful scenery")]
        public double? BeautifulScenery { get; private set; }

        [Display(Name = "Crowd-free")]
        public double? CrowdFree { get; private set; }

        #endregion

        #region Water

        [Display(Name = "Water purity")]
        public double? WaterPurity { get; private set; }

        [Display(Name = "Waste-free seabed")]
        public double? WasteFreeSeabed { get; private set; }

        [Display(Name = "Feet-friendly bottom")]
        public double? FeetFriendlyBottom { get; private set; }

        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; private set; }

        [Display(Name = "Coral reef wow factor")]
        public double? CoralReef { get; private set; }

        #endregion

        #region Activities

        [Display(Name = "Taking a walk")]
        public double? Walking { get; private set; }

        [Display(Name = "Snorkeling")]
        public double? Snorkeling { get; private set; }

        [Display(Name = "Kayaking")]
        public double? Kayaking { get; private set; }

        [Display(Name = "Camping")]
        public double? Camping { get; private set; }

        #endregion

        #region Tourist Infrastructure

        [Display(Name = "Environment-friendly infrastructure")]
        public double? Infrastructure { get; private set; }

        [Display(Name = "Long-term stay")]
        public double? LongTermStay { get; private set; }

        #endregion        

        public void UpdateTotalScore()
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

            score += (this.WaterPurity ?? 0);
            nullCount += ((this.WaterPurity == null) ? 1 : 0);

            score += (this.WasteFreeSeabed ?? 0);
            nullCount += ((this.WasteFreeSeabed == null) ? 1 : 0);

            score += (this.FeetFriendlyBottom ?? 0);
            nullCount += ((this.FeetFriendlyBottom == null) ? 1 : 0);

            score += (this.SeaLifeDiversity ?? 0);
            nullCount += ((this.SeaLifeDiversity == null) ? 1 : 0);

            score += (this.CoralReef ?? 0);
            nullCount += ((this.CoralReef == null) ? 1 : 0);

            score += (this.Walking ?? 0);
            nullCount += ((this.Walking == null) ? 1 : 0);

            score += (this.Snorkeling ?? 0);
            nullCount += ((this.Snorkeling == null) ? 1 : 0);

            score += (this.Kayaking ?? 0);
            nullCount += ((this.Kayaking == null) ? 1 : 0);

            score += (this.Camping ?? 0);
            nullCount += ((this.Camping == null) ? 1 : 0);          

            score += (this.Infrastructure ?? 0);
            nullCount += ((this.Infrastructure == null) ? 1 : 0);

            score += (this.LongTermStay ?? 0);
            nullCount += ((this.LongTermStay == null) ? 1 : 0);

            double? result = null;

            if (nullCount != BeachCriteriaCount)
            {
                result = Math.Round((double)(score / (BeachCriteriaCount - nullCount)), 1);
            }

            this.TotalScore = result;
        }
    }
}