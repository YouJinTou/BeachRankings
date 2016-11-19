namespace BeachRankings.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Blog
    {
        [Key]
        public string Id { get; set; }

        private ICollection<BlogArticle> blogArticles;

        public virtual User User { get; set; }

        [Required]
        public string Url { get; set; }

        public virtual ICollection<BlogArticle> BlogArticles
        {
            get
            {
                return this.blogArticles ?? (this.blogArticles = new HashSet<BlogArticle>());
            }
            protected set
            {
                this.blogArticles = value;
            }
        }
    }
}