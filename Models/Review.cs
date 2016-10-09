namespace BeachRankings.Models
{
    using BeachRankings.Models.Misc.Interfaces;
    using BeachRankings.Models.Misc.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Review : IRateable
    {
        [Key]
        public int Id { get; set; }

        public virtual User Author { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public int BeachId { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }
                
        public double? TotalScore { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(150, ErrorMessage = "150 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        [Criterion]
        [Display(Name = "Water quality")]
        public double? WaterQuality { get; set; }

        [Criterion]
        [Display(Name = "Seafloor cleanliness")]
        public double? SeafloorCleanliness { get; set; }

        [Criterion]
        [Display(Name = "Coral reef wow factor")]
        public double? CoralReefFactor { get; set; }

        [Criterion]
        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; set; }

        [Criterion]
        [Display(Name = "Good for snorkeling")]
        public double? SnorkelingSuitability { get; set; }

        [Criterion]
        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; set; }

        [Criterion]
        [Display(Name = "Crowd-free factor")]
        public double? CrowdFreeFactor { get; set; }        

        [Criterion]
        [Display(Name = "Sand quality")]
        public double? SandQuality { get; set; }

        [Criterion]
        [Display(Name = "Breathtaking environment")]
        public double? BreathtakingEnvironment { get; set; }

        [Criterion]
        [Display(Name = "Tent suitability")]
        public double? TentSuitability { get; set; }

        [Criterion]
        [Display(Name = "Suitable for kayaking")]
        public double? KayakSuitability { get; set; }

        [Criterion]
        [Display(Name = "Long stay suitability")]
        public double? LongStaySuitability { get; set; }        
    }
}