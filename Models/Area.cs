namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Area : IRegionSearchable
    {
        private ICollection<Beach> beaches;

        public Area()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The area field is required.")]
        [Index("IX_CountryRegionArea", IsUnique = true, Order = 1)]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        [Display(Name = "Area")]
        public string Name { get; set; }

        [Required]
        [Index("IX_CountryRegionArea", IsUnique = true, Order = 2)]
        public int CountryId { get; set; }

        public virtual Country Country { get; protected set; }

        [Required]
        [Index("IX_CountryRegionArea", IsUnique = true, Order = 3)]
        public int RegionId { get; set; }

        public virtual Region Region { get; protected set; }        

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