namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SecondaryDivision : IPlaceSearchable
    {
        private ICollection<TertiaryDivision> tertiaryDivisions;
        private ICollection<QuaternaryDivision> quaternaryDivisons;
        private ICollection<Beach> beaches;

        public SecondaryDivision()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The second-level division field is required.")]
        [Index("IX_CountrySecondary", IsUnique = true, Order = 1)]
        [MinLength(2, ErrorMessage = "The second-level division should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The second-level division cannot be longer than 100 characters.")]
        [Display(Name = "Second-level division")]
        public string Name { get; set; }

        public virtual Country Country { get; protected set; }

        [Required]
        [Index("IX_CountrySecondary", IsUnique = true, Order = 2)]
        public int CountryId { get; set; }
        
        public virtual PrimaryDivision PrimaryDivision { get; protected set; }

        public virtual WaterBody WaterBody { get; protected set; }

        public int? WaterBodyId { get; set; }

        [Required]
        [Index("IX_CountrySecondary", IsUnique = true, Order = 3)]
        public int PrimaryDivisionId { get; set; }

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