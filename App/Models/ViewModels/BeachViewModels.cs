namespace BeachRankings.App.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using BeachRankings.App.ValidationAttributes;

    public class AddBeachViewModel
    {
        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The country field is required.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "The region field is required.")]
        [Display(Name = "Region")]
        public int PrimaryDivisionId { get; set; }       

        [Display(Name = "Area")]
        public int? SecondaryDivisionId { get; set; }

        [Display(Name = "Sub-area")]
        public int? TertiaryDivisionId { get; set; }

        [Display(Name = "Locality")]
        public int? QuaternaryDivisionId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }        

        [RegularExpression(
            @"[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)", 
            ErrorMessage = "Invalid coordinates.")]
        public string Coordinates { get; set; }

        [ImagesValid(ErrorMessage = "Failed to upload images. Verify their count, size, and format.")]
        public IEnumerable<HttpPostedFileBase> Images { get; set; }
    }

    public class EditBeachViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The country field is required.")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "The region field is required.")]
        [Display(Name = "Region")]
        public int PrimaryDivisionId { get; set; }

        [Display(Name = "Area")]
        public int? SecondaryDivisionId { get; set; }

        [Display(Name = "Sub-area")]
        public int? TertiaryDivisionId { get; set; }

        [Display(Name = "Locality")]
        public int? QuaternaryDivisionId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }

        [RegularExpression(
            @"[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)",
            ErrorMessage = "Invalid coordinates.")]
        public string Coordinates { get; set; }

        public IEnumerable<SelectListItem> PrimaryDivisions { get; set; }

        public IEnumerable<SelectListItem> SecondaryDivisions { get; set; }

        public IEnumerable<SelectListItem> TertiaryDivisions { get; set; }

        public IEnumerable<SelectListItem> QuaternaryDivisions { get; set; }
    }

    public class ConciseBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string SecondaryDivision { get; set; }

        public double? TotalScore { get; set; }

        public int ReviewsCount { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }
    }

    public class DetailedBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public int CountryId { get; set; }

        public string PrimaryDivision { get; set; }

        public int PrimaryDivisionId { get; set; }

        public string SecondaryDivision { get; set; }

        public int SecondaryDivisionId { get; set; }

        public string TertiaryDivision { get; set; }

        public int TertiaryDivisionId { get; set; }

        public string QuaternaryDivision { get; set; }

        public int QuaternaryDivisionId { get; set; }

        public double? TotalScore { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string CreatorId { get; set; }

        public bool UserHasRated { get; set; }


        public string WaterBody { get; set; }

        public string Coordinates { get; set; }

        public IEnumerable<ConciseReviewViewModel> Reviews { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> Images { get; set; }
    }

    public class BeachTableRowViewModel : CriteriaBaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public int CountryId { get; set; }

        public string PrimaryDivision { get; set; }

        public int PrimaryDivisionId { get; set; }

        public string SecondaryDivision { get; set; }

        public int? SecondaryDivisionId { get; set; }

        public string TertiaryDivision { get; set; }

        public int? TertiaryDivisionId { get; set; }

        public string QuaternaryDivision { get; set; }

        public int? QuaternaryDivisionId { get; set; }

        public string WaterBody { get; set; }

        public int WaterBodyId { get; set; }

        public double? TotalScore { get; set; }
    }

    public class AutocompleteBeachViewModel : AutocompleteBaseViewModel
    {
        public string Country { get; set; }

        public string SecondaryDivision { get; set; }
    }    
}