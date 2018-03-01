namespace BeachRankings.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Watchlist
    {
        private ICollection<Beach> beaches;

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

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
