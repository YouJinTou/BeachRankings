namespace BeachRankings.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Beach
    {
        public Beach()
        {
            this.Reviews = new HashSet<Review>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The location field is required.")]
        [MinLength(2, ErrorMessage = "The location name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The location name cannot be longer than 100 characters.")]
        public string Location { get; set; }

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }

        public string Coordinates { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<BeachPhoto> Photos { get; set; }

        [Range(0, 10)]
        public double? TotalScore { get; set; }

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
    }
}