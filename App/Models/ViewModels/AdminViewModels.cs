namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class RestructureViewModel
    {
        [Display(Name = "Continent")]
        public int? ContinentId { get; set; }

        public string Continent { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }

        public string Country { get; set; }

        [Display(Name = "First")]
        public int? PrimaryDivisionId { get; set; }

        public string PrimaryDivision { get; set; }

        [Display(Name = "Second")]
        public int? SecondaryDivisionId { get; set; }

        public string SecondaryDivision { get; set; }

        [Display(Name = "Third")]
        public int? TertiaryDivisionId { get; set; }

        public string TertiaryDivision { get; set; }

        [Display(Name = "Fourth")]
        public int? QuaternaryDivisionId { get; set; }

        public string QuaternaryDivision { get; set; }
        
        [Display(Name = "Waterbody")]
        public int? WaterBodyId { get; set; }

        public IEnumerable<SelectListItem> WaterBodies { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Continents { get; set; }

        public string BeachIdsToMove { get; set; }
    }

    public class WeightViewModel
    {
        public int Id { get; set; }

        [Display()]
        public string Name { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Invalid range.")]
        public double Value { get; set; }
    }
}