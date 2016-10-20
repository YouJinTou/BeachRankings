namespace BeachRankings.Models
{
    using BeachRankings.Models.Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class WaterBody : IWaterBodySearchable
    {
        private ICollection<Beach> beaches;
        
        public WaterBody()
        {
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A body of water is required.")]
        [Index("IX_WaterBody", IsUnique = true)]
        [MinLength(2, ErrorMessage = "The name should be at least 2 characters long.")]
        [MaxLength(100, ErrorMessage = "There is no body of water with that long of a name.")]
        [Display(Name = "Body of water")]
        public string Name { get; set; }

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