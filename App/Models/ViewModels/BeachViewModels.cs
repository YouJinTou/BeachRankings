namespace App.Models.ViewModels
{
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ConciseBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public double? TotalScore { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }
    }

    public class DetailedBeachViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public double? TotalScore { get; set; }

        public string Description { get; set; }

        public string Coordinates { get; set; }

        [UIHint("ReviewViewModel.cshtml")]
        public IEnumerable<ReviewViewModel> Reviews { get; set; }

        public IEnumerable<BeachPhoto> Photos { get; set; }
    }
}