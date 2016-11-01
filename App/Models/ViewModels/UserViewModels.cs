namespace BeachRankings.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DashboardReviewViewModel : CriteriaBaseModel
    {
        public string BeachName { get; set; }

        [Range(0, 10)]
        public double? TotalScore { get; set; }

        public DateTime PostedOn { get; set; }
    }

    public class DashboardImageViewModel
    {
        public string BeachName { get; set; }

        public IEnumerable<BeachImageThumbnailViewModel> Paths { get; set; }
    }
}