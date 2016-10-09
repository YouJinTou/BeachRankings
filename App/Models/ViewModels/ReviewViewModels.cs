namespace App.Models.ViewModels
{
    using System;
    using System.ComponentModel;

    public class ReviewViewModel
    {
        public int Id { get; set; }

        [DisplayName("User")]
        public string UserName { get; set; }

        public string AvatarPath { get; set; }

        public DateTime PostedOn { get; set; }

        public double? TotalScore { get; set; }

        public string Content { get; set; }
    }
}