namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Location : ILocationSearchable
    {
        private ICollection<Beach> beaches;

        public Location()
        {
        }

        public Location(string name)
        {
            this.beaches = new HashSet<Beach>();

            this.Name = name;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required.")]
        [Index("IX_LocationName", IsUnique = true)]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
        [Display(Name = "Location")]
        public string Name { get; set; }

        public virtual ICollection<Beach> Beaches { get; set; }
    }
}