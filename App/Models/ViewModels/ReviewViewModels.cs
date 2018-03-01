namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.App.CustomAttributes;
    using BeachRankings.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class ReviewHeadViewModel
    {
        public int Id { get; set; }

        public int Upvotes { get; set; }

        [Range(0, 10)]
        public double? TotalScore { get; set; }

        public string BeachName { get; set; }

        public string PrimaryDivisionName { get; set; }

        public string SecondaryDivisionName { get; set; }

        public string AuthorId { get; set; }

        public bool AlreadyUpvoted { get; set; }
    }

    public class ReviewUserViewModel
    {
        public string UserName { get; set; }

        public UserBadge Badge { get; set; }

        public string AuthorId { get; set; }

        public string AvatarPath { get; set; }

        public bool IsBlogger { get; set; }

        public bool HasWatchlists { get; set; }

        public string BlogUrl { get; set; }

        public int ReviewsCount { get; set; }

        public int CountriesVisited { get; set; }

        public int Level { get; set; }

        public int ThanksReceived { get; set; }

        public DateTime PostedOn { get; set; }
    }

    public class ConciseReviewViewModel
    {
        public int Id { get; set; }

        public ReviewHeadViewModel ReviewHead { get; set; }

        public ReviewUserViewModel ReviewUser { get; set; }

        public bool IsBlogger { get; set; }

        public int Upvotes { get; set; }

        public IEnumerable<BlogArticleViewModel> BlogArticles { get; set; }

        public string Content { get; set; }

        public HorizontalCriteriaViewModel Criteria { get; set; }

        public DateTime PostedOn { get; set; }
    }

    public class DetailedReviewViewModel : CriteriaBaseModel
    {
        public ReviewHeadViewModel ReviewHead { get; set; }

        public ReviewUserViewModel ReviewUser { get; set; }

        public bool IsBlogger { get; set; }

        public string Content { get; set; }

        public IEnumerable<BlogArticleViewModel> BlogArticles { get; set; }

        [Range(0, 10)]
        public double? TotalScore { get; set; }

        public DetailedBeachViewModel BeachHead { get; set; }        
    }

    public class PostReviewViewModel : CriteriaBaseModel
    {
        public DetailedBeachViewModel BeachHead { get; set; }

        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        public bool IsBlogger { get; set; }

        [Display(Name = "Blog article links")]
        [UrisValid(ErrorMessage = "We couldn't process the provided links.")]
        [ArticleLinksValid]
        public string ArticleLinks { get; set; }
        
        public int BeachReviewsCount { get; set; }

        [ImagesValid(ErrorMessage = "Failed to upload images. Verify their total size and format.")]
        public IEnumerable<HttpPostedFileBase> Images { get; set; }

        public CriteriaViewModel Criteria { get; set; }
    }

    public class EditReviewViewModel : CriteriaBaseModel
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        [MaxLength(3000, ErrorMessage = "We'll happily accept 3000 symbols and below.")]
        [Display(Name = "Review")]
        public string Content { get; set; }

        public bool IsBlogger { get; set; }

        [Display(Name = "Blog article links")]
        [UrisValid(ErrorMessage = "We couldn't process the provided links.")]
        [ArticleLinksValid]
        public string ArticleLinks { get; set; }

        public DetailedBeachViewModel BeachHead { get; set; }
        
        public int BeachReviewsCount { get; set; }

        public CriteriaViewModel Criteria { get; set; }
    }
}