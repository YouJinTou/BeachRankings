namespace BeachRankings.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        private const int BeachCriteriaCount = 12;

        public Review()
        {
        }
        
        public Review(
            int beachId,
            string content,
            double? waterQuality,
            double? seafloorCleanliness,
            double? coralReefFactor,
            double? seaLifeDiversity,
            double? snorkelingSuitability,
            double? beachCleanliness,
            double? crowdFreeFactor,
            double? sandQuality,
            double? breathtakingEnvironment,
            double? tentSuitability,
            double? kayakSuitability,
            double? longStaySuitability            
            )
        {
            this.BeachId = beachId;
            this.PostedOn = DateTime.Now;
            this.Content = content;
            this.WaterQuality = waterQuality;
            this.SeafloorCleanliness = seafloorCleanliness;
            this.CoralReefFactor = coralReefFactor;
            this.SeaLifeDiversity = seaLifeDiversity;
            this.SnorkelingSuitability = snorkelingSuitability;
            this.BeachCleanliness = beachCleanliness;
            this.CrowdFreeFactor = crowdFreeFactor;
            this.SandQuality = sandQuality;
            this.BreathtakingEnvironment = breathtakingEnvironment;
            this.TentSuitability = tentSuitability;
            this.KayakSuitability = kayakSuitability;
            this.LongStaySuitability = longStaySuitability;

            this.CalculateTotalScore();
        }

        [Key]
        public int Id { get; set; }

        public virtual User Author { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public int BeachId { get; private set; }
        
        [Required]
        public DateTime PostedOn { get; private set; }
                
        public double? TotalScore { get; private set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(100, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; private set; }        

        [Display(Name = "Water quality")]
        public double? WaterQuality { get; private set; }

        [Display(Name = "Seafloor cleanliness")]
        public double? SeafloorCleanliness { get; private set; }

        [Display(Name = "Coral reef wow factor")]
        public double? CoralReefFactor { get; private set; }

        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; private set; }

        [Display(Name = "Good for snorkeling")]
        public double? SnorkelingSuitability { get; private set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; private set; }

        [Display(Name = "Crowd-free factor")]
        public double? CrowdFreeFactor { get; private set; }        

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; private set; }

        [Display(Name = "Breathtaking environment")]
        public double? BreathtakingEnvironment { get; private set; }

        [Display(Name = "Tent suitability")]
        public double? TentSuitability { get; private set; }

        [Display(Name = "Suitable for kayaking")]
        public double? KayakSuitability { get; private set; }

        [Display(Name = "Long stay suitability")]
        public double? LongStaySuitability { get; private set; }

        private void CalculateTotalScore()
        {
            double score = 0;
            int nullCount = 0; // Count of criteria NOT voted for

            score += (this.WaterQuality ?? 0);
            nullCount += ((this.WaterQuality == null) ? 1 : 0);

            score += (this.SeafloorCleanliness ?? 0);
            nullCount += ((this.SeafloorCleanliness == null) ? 1 : 0);

            score += (this.CoralReefFactor ?? 0);
            nullCount += ((this.CoralReefFactor == null) ? 1 : 0);

            score += (this.SeaLifeDiversity ?? 0);
            nullCount += ((this.SeaLifeDiversity == null) ? 1 : 0);

            score += (this.SnorkelingSuitability ?? 0);
            nullCount += ((this.SnorkelingSuitability == null) ? 1 : 0);

            score += (this.BeachCleanliness ?? 0);
            nullCount += ((this.BeachCleanliness == null) ? 1 : 0);

            score += (this.CrowdFreeFactor ?? 0);
            nullCount += ((this.CrowdFreeFactor == null) ? 1 : 0);

            score += (this.SandQuality ?? 0);
            nullCount += ((this.SandQuality == null) ? 1 : 0);

            score += (this.BreathtakingEnvironment ?? 0);
            nullCount += ((this.BreathtakingEnvironment == null) ? 1 : 0);

            score += (this.TentSuitability ?? 0);
            nullCount += ((this.TentSuitability == null) ? 1 : 0);

            score += (this.KayakSuitability ?? 0);
            nullCount += ((this.KayakSuitability == null) ? 1 : 0);

            score += (this.LongStaySuitability ?? 0);
            nullCount += ((this.LongStaySuitability == null) ? 1 : 0);

            double? result = null;

            if (nullCount != BeachCriteriaCount)
            {
                result = Math.Round((double)(score / (BeachCriteriaCount - nullCount)), 1);
            }

            this.TotalScore = result;
        }
    }
}