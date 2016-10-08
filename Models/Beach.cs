namespace BeachRankings.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Beach
    {
        public Beach()
        {
            this.Reviews = new HashSet<Review>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The location field is required.")]
        [MinLength(2, ErrorMessage = "The location name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The location name cannot be longer than 100 characters.")]
        public string Location { get; set; }

        [Required]
        [Range(0, 100)]
        public int TotalScore { get; set; }

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }

        [UIHint("Separate the latitude and longitude with a semicolon(;).")]
        public string Coordinates { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<BeachPhoto> Photos { get; set; }
    }
}