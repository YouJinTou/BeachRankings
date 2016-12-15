namespace BeachRankings.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BlogArticle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string BlogId { get; set; }

        public virtual Blog Blog { get; protected set; }

        [Required]
        public int ReviewId { get; set; }

        public virtual Review Review { get; protected set; }

        [Required]
        public int BeachId { get; set; }

        public virtual Beach Beach { get; set; }

        [Required]
        public string Url { get; set; }

        public string Title { get; set; }
    }
}