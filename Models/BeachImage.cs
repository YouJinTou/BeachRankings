namespace BeachRankings.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BeachImage
    {
        public BeachImage()
        {
            this.UploadedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public virtual User Author { get; protected set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public int BeachId { get; set; }

        public virtual Beach Beach { get; protected set; }

        [Required]
        public DateTime UploadedOn { get; private set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Name { get; set; }
    }
}