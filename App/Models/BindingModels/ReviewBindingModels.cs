namespace BeachRankings.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class PostReviewBindingModel
    {
        [Required]
        public int BeachId { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(100, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        public string Content { get; set; }

        #region Beachline

        public double? SandQuality { get; set; }

        public double? BeachCleanliness { get; set; }

        public double? BeautifulScenery { get; set; }

        public double? CrowdFree { get; set; }

        #endregion

        #region Water

        public double? WaterPurity { get; set; }

        public double? WasteFreeSeabed { get; set; }

        public double? FeetFriendlyBottom { get; set; }

        public double? SeaLifeDiversity { get; set; }

        public double? CoralReef { get; set; }

        #endregion

        #region Activities

        public double? Walking { get; set; }

        public double? Snorkeling { get; set; }

        public double? Kayaking { get; set; }

        public double? Camping { get; set; }

        #endregion

        #region Tourist Infrastructure

        public double? Infrastructure { get; set; }

        public double? LongTermStay { get; set; }

        #endregion
    }

    public class EditReviewBindingModel
    {
        [Required]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(150, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        public string Content { get; set; }

        #region Beachline

        public double? SandQuality { get; set; }

        public double? BeachCleanliness { get; set; }

        public double? BeautifulScenery { get; set; }

        public double? CrowdFree { get; set; }

        #endregion

        #region Water

        public double? WaterPurity { get; set; }

        public double? WasteFreeSeabed { get; set; }

        public double? FeetFriendlyBottom { get; set; }

        public double? SeaLifeDiversity { get; set; }

        public double? CoralReef { get; set; }

        #endregion

        #region Activities

        public double? Walking { get; set; }

        public double? Snorkeling { get; set; }

        public double? Kayaking { get; set; }

        public double? Camping { get; set; }

        #endregion

        #region Tourist Infrastructure

        public double? Infrastructure { get; set; }

        public double? LongTermStay { get; set; }

        #endregion
    }
}