namespace BeachRankings.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Blog
    {
        [Key, ForeignKey("User")]
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