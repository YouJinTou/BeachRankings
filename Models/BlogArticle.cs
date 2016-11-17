namespace BeachRankings.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BlogArticle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BlogId { get; set; }

        [Required]
        public string Url { get; set; }

        public virtual Blog Blog { get; set; }
    }
}