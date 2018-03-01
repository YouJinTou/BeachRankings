namespace BeachRankings.App.Models.ViewModels
{
    using BeachRankings.App.CustomAttributes;
    using BeachRankings.Models.Enums;
    using System.Collections.Generic;
    using System.Web;

    public class DashboardImageViewModel
    {
        public string BeachName { get; set; }

        public IEnumerable<DashboardBeachImageThumbnailViewModel> Paths { get; set; }
    }

    public class TableUserReviewsViewModel
    {
        public ContributorVerticalViewModel Contributor { get; set; }

        public IEnumerable<BeachRowViewModel> Reviews { get; set; }
    }

    public class ChangeAvatarViewModel
    {
        public string AvatarPath { get; set; }

        [UserAvatarValid]
        public HttpPostedFileBase Avatar { get; set; }
    }

    public class ContributorRowViewModel
    {
        public string Id { get; set; }

        public int Level { get; set; }

        public string UserName { get; set; }

        public UserBadge Badge { get; set; }

        public int BlogPostsCount { get; set; }

        public IDictionary<string, int> BeachesByCountry { get; set; }

        public string AvatarPath { get; set; }

        public int ReviewsCount { get; set; }

        public int CountriesVisited { get; set; }

        public int ThanksReceived { get; set; }

        public bool IsBlogger { get; set; }

        public string BlogUrl { get; set; }
    }

    public class ContributorVerticalViewModel
    {
        public string Id { get; set; }

        public int Rank { get; set; }

        public int Level { get; set; }

        public string UserName { get; set; }

        public string AvatarPath { get; set; }

        public int ReviewsCount { get; set; }

        public int CountriesVisited { get; set; }

        public int ThanksReceived { get; set; }

        public bool IsBlogger { get; set; }

        public string BlogUrl { get; set; }
    }
}