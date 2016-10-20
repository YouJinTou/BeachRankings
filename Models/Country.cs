namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Country : ICountrySearchable
    {
        private ICollection<Location> locations;
        private ICollection<Beach> beaches;

        [Key]
        public int Id { get; set; }

        [Required]
        [Index("IX_CountryName", IsUnique = true)]
        [MaxLength(100)]
        [Display(Name = "Country")]
        public string Name { get; set; }

        public virtual ICollection<Location> Locations
        {
            get
            {
                return this.locations ?? (this.locations = new HashSet<Location>());
            }
            protected set
            {
                this.locations = value;
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