namespace BeachRankings.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Score
    {
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
    }
}
