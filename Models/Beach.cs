namespace BeachRankings.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    public class Beach
    {
        private ICollection<Review> reviews;
        private ICollection<BeachPhoto> photos;

        public Beach()
        {
        }

        public Beach(string name, string location)
        {
            this.reviews = new HashSet<Review>();
            this.photos = new HashSet<BeachPhoto>();

            this.Name = name;
            this.Location = location;
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

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }

        public string Coordinates { get; set; }

        public virtual ICollection<Review> Reviews
        {
            get
            {
                return this.reviews;
            }
            set
            {
                this.reviews = value;
            }
        }

        public virtual ICollection<BeachPhoto> Photos
        {
            get
            {
                return this.photos;
            }
            set
            {
                this.photos = value;
            }
        }

        [Range(0, 10)]
        public double? TotalScore { get; private set; }

        [Display(Name = "Water quality")]
        public double? WaterQuality { get; private set; }

        [Display(Name = "Seafloor cleanliness")]
        public double? SeafloorCleanliness { get; private set; }

        [Display(Name = "Coral reef wow factor")]
        public double? CoralReefFactor { get; private set; }

        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; private set; }

        [Display(Name = "Good for snorkeling")]
        public double? SnorkelingSuitability { get; private set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; private set; }

        [Display(Name = "Crowd-free factor")]
        public double? CrowdFreeFactor { get; private set; }

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; private set; }

        [Display(Name = "Breathtaking environment")]
        public double? BreathtakingEnvironment { get; private set; }

        [Display(Name = "Tent suitability")]
        public double? TentSuitability { get; private set; }

        [Display(Name = "Suitable for kayaking")]
        public double? KayakSuitability { get; private set; }

        [Display(Name = "Long stay suitability")]
        public double? LongStaySuitability { get; private set; }
                
        public void UpdateScores()
        {
            this.WaterQuality = this.Reviews.Average(r => r.WaterQuality);
            this.SeafloorCleanliness = this.Reviews.Average(r => r.SeafloorCleanliness);
            this.CoralReefFactor = this.Reviews.Average(r => r.CoralReefFactor);
            this.SeaLifeDiversity = this.Reviews.Average(r => r.SeaLifeDiversity);
            this.SnorkelingSuitability = this.Reviews.Average(r => r.SnorkelingSuitability);
            this.BeachCleanliness = this.Reviews.Average(r => r.BeachCleanliness);
            this.CrowdFreeFactor = this.Reviews.Average(r => r.CrowdFreeFactor);
            this.SandQuality = this.Reviews.Average(r => r.SandQuality);
            this.BreathtakingEnvironment = this.Reviews.Average(r => r.BreathtakingEnvironment);
            this.TentSuitability = this.Reviews.Average(r => r.TentSuitability);
            this.KayakSuitability = this.Reviews.Average(r => r.KayakSuitability);
            this.LongStaySuitability = this.Reviews.Average(r => r.LongStaySuitability);
            this.TotalScore = this.Reviews.Average(r => r.TotalScore);
        }
    }
}