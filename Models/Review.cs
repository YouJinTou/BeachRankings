namespace BeachRankings.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        [Key]
        public int Id { get; set; }        

        [Required]
        [MinLength(150, ErrorMessage = "Two sentences should get you to 150 characaters, don't you think?")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        [Display(Name = "Water quality")]
        public double? WaterQuality { get; set; }

        [Display(Name = "Seafloor cleanliness")]
        public double? SeafloorCleanliness { get; set; }

        [Display(Name = "Coral reef wow factor")]
        public double? CoralReefFactor { get; set; }

        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; set; }

        [Display(Name = "Good for snorkeling")]
        public double? SnorkelingSuitability { get; set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; set; }

        [Display(Name = "Crowd-free factor")]
        public double? CrowdFreeFactor { get; set; }        

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; set; }

        [Display(Name = "Breathtaking environment")]
        public double? BreathtakingEnvironment { get; set; }

        [Display(Name = "Tent suitability")]
        public double? TentSuitability { get; set; }

        [Display(Name = "Suitable for kayaking")]
        public double? KayakSuitability { get; set; }

        [Display(Name = "Long stay suitability")]
        public double? LongStaySuitability { get; set; }

        public virtual User Author { get; set; }

        [Required]
        public string AuthorId { get; set; }
    }
}