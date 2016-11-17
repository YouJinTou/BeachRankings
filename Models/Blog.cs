namespace BeachRankings.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Url { get; set; }
    }
}