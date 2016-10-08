namespace App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class PostReviewBindingModel
    {
        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(150, ErrorMessage = "150 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        public string Content { get; set; }

        public double? WaterQuality { get; set; }

        public double? SeafloorCleanliness { get; set; }

        public double? CoralReefFactor { get; set; }

        public double? SeaLifeDiversity { get; set; }

        public double? SnorkelingSuitability { get; set; }

        public double? BeachCleanliness { get; set; }

        public double? CrowdFreeFactor { get; set; }

        public double? SandQuality { get; set; }

        public double? BreathtakingEnvironment { get; set; }

        public double? TentSuitability { get; set; }

        public double? KayakSuitability { get; set; }

        public double? LongStaySuitability { get; set; }
    }
}