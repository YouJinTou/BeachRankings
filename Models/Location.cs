namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Location : ILocationSearchable
    {
        private string name;
        private LocationType locationType;
        private ICollection<Beach> beaches;

        public Location()
        {
        }

        public Location(string name, LocationType locationType)
        {
            this.name = name;
            this.locationType = locationType;
            this.beaches = new HashSet<Beach>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The location field is required.")]
        [Index("IX_LocationName", IsUnique = true)]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        [Display(Name = "Location")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        [Required]
        public LocationType LocationType
        {
            get
            {
                return this.locationType;
            }
            set
            {
                this.locationType = value;
            }
        }

        public virtual ICollection<Beach> Beaches
        {
            get
            {
                return this.beaches;
            }
            set
            {
                this.beaches = value;
            }
        }
    }
}