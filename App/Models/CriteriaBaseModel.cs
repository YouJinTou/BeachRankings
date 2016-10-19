namespace BeachRankings.App.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CriteriaBaseModel
    {
        #region Beachline

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; set; }

        [Display(Name = "Beautiful scenery")]
        public double? BeautifulScenery { get; set; }

        [Display(Name = "Crowd-free")]
        public double? CrowdFree { get; set; }

        #endregion

        #region Water

        [Display(Name = "Water purity")]
        public double? WaterPurity { get; set; }

        [Display(Name = "Waste-free seabed")]
        public double? WasteFreeSeabed { get; set; }

        [Display(Name = "Feet-friendly bottom")]
        public double? FeetFriendlyBottom { get; set; }

        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; set; }

        [Display(Name = "Coral reef wow factor")]
        public double? CoralReef { get; set; }

        #endregion

        #region Activities

        [Display(Name = "Taking a walk")]
        public double? Walking { get; set; }

        [Display(Name = "Snorkeling")]
        public double? Snorkeling { get; set; }

        [Display(Name = "Kayaking")]
        public double? Kayaking { get; set; }

        [Display(Name = "Camping")]
        public double? Camping { get; set; }

        #endregion

        #region Tourist Infrastructure

        [Display(Name = "Environment-friendly infrastructure")]
        public double? Infrastructure { get; set; }

        [Display(Name = "Long-term stay")]
        public double? LongTermStay { get; set; }

        #endregion        
    }
}