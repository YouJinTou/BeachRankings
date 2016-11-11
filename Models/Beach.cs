﻿namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Beach : IBeachSearchable
    {
        private ICollection<Review> reviews;
        private ICollection<BeachImage> images;

        public Beach()
        {
            this.AddedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        [Index("IX_PrimaryBeach", IsUnique = true, Order = 1)]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [Required]
        public DateTime AddedOn { get; private set; }

        [Required]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        [Index("IX_PrimaryBeach", IsUnique = true, Order = 2)]
        public int? PrimaryDivisionId { get; set; }

        public virtual PrimaryDivision PrimaryDivision { get; protected set; }

        public int? SecondaryDivisionId { get; set; }

        public virtual SecondaryDivision SecondaryDivision { get; protected set; }

        public int? TertiaryDivisionId { get; set; }

        public virtual TertiaryDivision TertiaryDivision { get; protected set; }
        
        public int? QuaternaryDivisionId { get; set; }

        public virtual QuaternaryDivision QuaternaryDivision { get; protected set; }

        [Required]
        public int WaterBodyId { get; set; }

        public virtual WaterBody WaterBody { get; protected set; }

        [MaxLength(350, ErrorMessage = "The description cannot be longer than 350 characters.")]
        public string Description { get; set; }

        public string Address { get; set; }

        public string Coordinates { get; set; }

        public virtual ICollection<Review> Reviews
        {
            get
            {
                return this.reviews ?? (this.reviews = new HashSet<Review>());
            }
            protected set
            {
                this.reviews = value;
            }
        }

        public virtual ICollection<BeachImage> Images
        {
            get
            {
                return this.images ?? (this.images = new HashSet<BeachImage>());
            }
            protected set
            {
                this.images = value;
            }
        }

        [Range(0, 10)]
        public double? TotalScore { get; private set; }

        #region Beachline

        [Display(Name = "Sand quality")]
        public double? SandQuality { get; private set; }

        [Display(Name = "Beach cleanliness")]
        public double? BeachCleanliness { get; private set; }

        [Display(Name = "Beautiful scenery")]
        public double? BeautifulScenery { get; private set; }

        [Display(Name = "Crowd-free")]
        public double? CrowdFree { get; private set; }

        #endregion

        #region Water

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

        public void SetBeachData()
        {
            var interval = "-";
            var secondaryDivisionName = (this.SecondaryDivision == null) ? null : this.SecondaryDivision.Name + interval;
            var tertiaryDivisionName = (this.TertiaryDivision == null) ? null : this.TertiaryDivision.Name + interval;
            var quaternaryDivisinoName = (this.QuaternaryDivision == null) ? null : this.QuaternaryDivision.Name + interval;
            this.Address = (
                this.Country.Name + interval + 
                this.PrimaryDivision.Name + interval +
                secondaryDivisionName +
                tertiaryDivisionName +
                quaternaryDivisinoName +
                this.WaterBody.Name)
                .Trim();
        }

        public void UpdateScores()
        {
            this.SandQuality = this.RoundScore(this.Reviews.Average(r => r.SandQuality));
            this.BeachCleanliness = this.RoundScore(this.Reviews.Average(r => r.BeachCleanliness));
            this.BeautifulScenery = this.RoundScore(this.Reviews.Average(r => r.BeautifulScenery));
            this.CrowdFree = this.RoundScore(this.Reviews.Average(r => r.CrowdFree));

            this.WaterPurity = this.RoundScore(this.Reviews.Average(r => r.WaterPurity));
            this.WasteFreeSeabed = this.RoundScore(this.Reviews.Average(r => r.WasteFreeSeabed));
            this.FeetFriendlyBottom = this.RoundScore(this.Reviews.Average(r => r.FeetFriendlyBottom));
            this.SeaLifeDiversity = this.RoundScore(this.Reviews.Average(r => r.SeaLifeDiversity));
            this.CoralReef = this.RoundScore(this.Reviews.Average(r => r.CoralReef));

            this.Walking = this.RoundScore(this.Reviews.Average(r => r.Walking));
            this.Snorkeling = this.RoundScore(this.Reviews.Average(r => r.Snorkeling));
            this.Kayaking = this.RoundScore(this.Reviews.Average(r => r.Kayaking));
            this.Camping = this.RoundScore(this.Reviews.Average(r => r.Camping));

            this.Infrastructure = this.RoundScore(this.Reviews.Average(r => r.Infrastructure));
            this.LongTermStay = this.RoundScore(this.Reviews.Average(r => r.LongTermStay));

            this.TotalScore = this.RoundScore(this.Reviews.Average(r => r.TotalScore));
        }

        private double? RoundScore(double? score)
        {
            if (!score.HasValue)
            {
                return null;
            }

            return Math.Round(score.Value, 1);
        }
    }
}