﻿namespace BeachRankings.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
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

        [Display(Name = "Body of water")]
        public string WaterBody { get; set; }

        public string ApproximateAddress { get; set; }

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

        #region Beach

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; private set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; private set; }

        [Display(Name = "Beautiful scenery")]
        public double? BeautifulScenery { get; private set; }

        [Display(Name = "Crowd-free")]
        public double? CrowdFree { get; private set; }

        #endregion

        #region Sea

        [Display(Name = "Water purity")]
        public double? WaterPurity { get; private set; }

        [Display(Name = "Waste-free seabed")]
        public double? WasteFreeSeabed { get; private set; }

        [Display(Name = "Feet-friendly bottom")]
        public double? FeetFriendlyBottom { get; private set; }

        [Display(Name = "Sea life diversity")]
        public double? SeaLifeDiversity { get; private set; }

        [Display(Name = "Coral reef wow factor")]
        public double? CoralReef { get; private set; }

        #endregion

        #region Activities

        [Display(Name = "Taking a walk")]
        public double? Walking { get; private set; }

        [Display(Name = "Snorkeling")]
        public double? Snorkeling { get; private set; }

        [Display(Name = "Kayaking")]
        public double? Kayaking { get; private set; }

        [Display(Name = "Camping")]
        public double? Camping { get; private set; }

        #endregion

        #region Tourist Infrastructure

        [Display(Name = "Environment-friendly infrastructure")]
        public double? Infrastructure { get; private set; }

        [Display(Name = "Long-term stay")]
        public double? LongTermStay { get; private set; }

        #endregion

        public void UpdateScores()
        {
            this.SandQuality = this.Reviews.Average(r => r.SandQuality);
            this.BeachCleanliness = this.Reviews.Average(r => r.BeachCleanliness);
            this.BeautifulScenery = this.Reviews.Average(r => r.BeautifulScenery);
            this.CrowdFree = this.Reviews.Average(r => r.CrowdFree);

            this.WaterPurity = this.Reviews.Average(r => r.WaterPurity);
            this.WasteFreeSeabed = this.Reviews.Average(r => r.WasteFreeSeabed);
            this.FeetFriendlyBottom = this.Reviews.Average(r => r.FeetFriendlyBottom);
            this.SeaLifeDiversity = this.Reviews.Average(r => r.SeaLifeDiversity);
            this.CoralReef = this.Reviews.Average(r => r.CoralReef);

            this.Walking = this.Reviews.Average(r => r.Walking);
            this.Snorkeling = this.Reviews.Average(r => r.Snorkeling);
            this.Kayaking = this.Reviews.Average(r => r.Kayaking);
            this.Camping = this.Reviews.Average(r => r.Camping);

            this.Infrastructure = this.Reviews.Average(r => r.Infrastructure);
            this.LongTermStay = this.Reviews.Average(r => r.LongTermStay);

            this.TotalScore = this.Reviews.Average(r => r.TotalScore);
        }
    }
}