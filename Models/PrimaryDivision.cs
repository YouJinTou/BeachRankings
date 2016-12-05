namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PrimaryDivision : IPlaceSearchable
    {
        private ICollection<SecondaryDivision> secondaryDivisions;
        private ICollection<TertiaryDivision> tertiaryDivisions;
        private ICollection<QuaternaryDivision> quaternaryDivisons;
        private ICollection<Beach> beaches;

        public PrimaryDivision()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The first-level division field is required.")]
        [Index("IX_CountryPrimary", IsUnique = true, Order = 0)]
        [MinLength(2, ErrorMessage = "The first-level division should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The first-level division cannot be longer than 100 characters.")]
        [Display(Name = "First-level division")]
        public string Name { get; set; }

        public int? ContinentId { get; set; }

        public virtual Continent Continent { get; set; }

        [Required]
        [Index("IX_CountryPrimary", IsUnique = true, Order = 1)]
        public int CountryId { get; set; }

        public virtual Country Country { get; protected set; }

        public int? WaterBodyId { get; set; }

        public virtual WaterBody WaterBody { get; protected set; }

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
            set
            {
                this.beaches = value;
            }
        }
    }
}