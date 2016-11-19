namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.App.CustomAttributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ConciseReviewViewModel
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public string UserName { get; set; }

        public bool IsBlogger { get; set; }

        public string BlogUrl { get; set; }

        public IEnumerable<BlogArticleViewModel> BlogArticles { get; set; }

        public int ReviewsCount { get; set; }

        public int CountriesVisited { get; set; }

        public int Level { get; set; }

        public string AvatarPath { get; set; }

        public DateTime PostedOn { get; set; }

        [Range(0, 10)]
        public double? TotalScore { get; set; }

        public string Content { get; set; }

        public int Upvotes { get; set; }

        public bool AlreadyUpvoted { get; set; }
    }

    public class DetailedReviewViewModel : CriteriaBaseModel
    {
        [Range(0, 10)]
        public double? TotalScore { get; set; }
    }

    public class PostReviewViewModel : CriteriaBaseModel
    {
        public int BeachId { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(100, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        public bool IsBlogger { get; set; }

        [Display(Name = "Blog article links")]
        [UrisValid(ErrorMessage = "We couldn't process the provided links.")]
        public string ArticleLinks { get; set; }

        public string BeachName { get; set; }

        public string BeachCountry { get; set; }

        public int CountryId { get; set; }

        public string BeachPrimaryDivision { get; set; }

        public int? PrimaryDivisionId { get; set; }

        public string BeachSecondaryDivision { get; set; }

        public int? SecondaryDivisionId { get; set; }

        public double? BeachTotalScore { get; set; }

        public int BeachReviewsCount { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> BeachImagePaths { get; set; }
    }

    public class EditReviewViewModel : CriteriaBaseModel
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        [Required(ErrorMessage = "The review field is required.")]
        [MinLength(100, ErrorMessage = "100 characters should be doable.")]
        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        public bool IsBlogger { get; set; }

        [Display(Name = "Blog article links")]
        [UrisValid(ErrorMessage = "We couldn't process the provided links.")]
        public string ArticleLinks { get; set; }

        public int BeachId { get; set; }

        public string BeachName { get; set; }

        public string BeachCountry { get; set; }

        public int CountryId { get; set; }

        public string BeachPrimaryDivision { get; set; }

        public int? PrimaryDivisionId { get; set; }

        public string BeachSecondaryDivision { get; set; }

        public int? SecondaryDivisionId { get; set; }

        public double? BeachTotalScore { get; set; }

        public int BeachReviewsCount { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> BeachImagePaths { get; set; }                
    }
}