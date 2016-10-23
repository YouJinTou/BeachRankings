namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Country : ICountrySearchable
    {
        private ICollection<Region> regions;
        private ICollection<Area> areas;
        private ICollection<Beach> beaches;

        [Key]
        public int Id { get; set; }

        [Required]
        [Index("IX_Country", IsUnique = true)]
        [MaxLength(100)]
        [Display(Name = "Country")]
        public string Name { get; set; }

        public virtual ICollection<Region> Regions
        {
            get
            {
                return this.regions ?? (this.regions = new HashSet<Region>());
            }
            protected set
            {
                this.regions = value;
            }
        }

        public virtual ICollection<Area> Areas
        {
            get
            {
                return this.areas ?? (this.areas = new HashSet<Area>());
            }
            protected set
            {
                this.areas = value;
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