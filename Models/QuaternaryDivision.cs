namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class QuaternaryDivision : IPlaceSearchable
    {
        private ICollection<Beach> beaches;

        public QuaternaryDivision()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The fourth-level division field is required.")]
        [Index("IX_CountryQuaternary", IsUnique = true, Order = 0)]
        [MinLength(2, ErrorMessage = "The fourth-level division should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The fourth-level division cannot be longer than 100 characters.")]
        [Display(Name = "Fourth-level division")]
        public string Name { get; set; }

        [Required]
        public int ContinentId { get; set; }

        public virtual Continent Continent { get; set; }

        [Required]
        [Index("IX_CountryQuaternary", IsUnique = true, Order = 1)]
        public int CountryId { get; set; }

        public virtual Country Country { get; protected set; }

        [Required]
        [Index("IX_CountryQuaternary", IsUnique = true, Order = 2)]
        public int PrimaryDivisionId { get; set; }

        public virtual PrimaryDivision PrimaryDivision { get; protected set; }

        [Required]
        public int SecondaryDivisionId { get; set; }

        public virtual SecondaryDivision SecondaryDivision { get; protected set; }

        [Required]
        public int TertiaryDivisionId { get; set; }

        public virtual TertiaryDivision TertiaryDivision { get; protected set; }

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