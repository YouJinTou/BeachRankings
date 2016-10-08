namespace BeachRankings.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BeachPhoto
    {
        [Key]
        public int Id { get; set; }

        public virtual User Author { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public int BeachId { get; set; }

        [Required]
        public DateTime UploadedOn { get; set; }

        [Required]
        public string Path { get; set; }

        [MaxLength(250, ErrorMessage = "The description can be up to 250 characters long.")]
        public string Description { get; set; }   
    }
}