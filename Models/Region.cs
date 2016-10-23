namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Region : IRegionSearchable
    {
        private ICollection<Area> areas;
        private ICollection<Beach> beaches;

        public Region()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The region field is required.")]
        [Index("IX_CountryRegion", IsUnique = true, Order = 1)]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        [Display(Name = "Region")]
        public string Name { get; set; }

        [Required]
        [Index("IX_CountryRegion", IsUnique = true, Order = 2)]
        public int CountryId { get; set; }

        public virtual Country Country { get; protected set; }

        [Required]
        public int WaterBodyId { get; set; }

        public virtual WaterBody WaterBody { get; set; }

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