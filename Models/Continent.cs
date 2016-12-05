﻿namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Continent : IPlaceSearchable
    {
        private ICollection<Country> countries;
        private ICollection<PrimaryDivision> primaryDivisions;
        private ICollection<SecondaryDivision> secondaryDivisions;
        private ICollection<TertiaryDivision> tertiaryDivisions;
        private ICollection<QuaternaryDivision> quaternaryDivisons;
        private ICollection<Beach> beaches;

        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(30)]
        [Display(Name = "Continent")]
        public string Name { get; set; }
        
        public virtual ICollection<Country> Countries
        {
            get
            {
                return this.countries ?? (this.countries = new HashSet<Country>());
            }
            set
            {
                this.countries = value;
            }
        }


        public virtual ICollection<PrimaryDivision> PrimaryDivisions
        {
            get
            {
                return this.primaryDivisions ?? (this.primaryDivisions = new HashSet<PrimaryDivision>());
            }
            set
            {
                this.primaryDivisions = value;
            }
        }

        public virtual ICollection<SecondaryDivision> SecondaryDivisions
        {
            get
            {
                return this.secondaryDivisions ?? (this.secondaryDivisions = new HashSet<SecondaryDivision>());
            }
            set
            {
                this.secondaryDivisions = value;
            }
        }

        public virtual ICollection<TertiaryDivision> TertiaryDivisions
        {
            get
            {
                return this.tertiaryDivisions ?? (this.tertiaryDivisions = new HashSet<TertiaryDivision>());
            }
            set
            {
                this.tertiaryDivisions = value;
            }
        }

        public virtual ICollection<QuaternaryDivision> QuaternaryDivisions
        {
            get
            {
                return this.quaternaryDivisons ?? (this.quaternaryDivisons = new HashSet<QuaternaryDivision>());
            }
            set
            {
                this.quaternaryDivisons = value;
            }
        }        

        public virtual ICollection<Beach> Beaches
        {
            get
            {
                return this.beaches ?? (this.beaches = new HashSet<Beach>());
            }
            protected set
            {
                this.beaches = value;
            }
        }
    }
}