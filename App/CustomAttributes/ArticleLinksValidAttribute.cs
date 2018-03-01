namespace BeachRankings.App.CustomAttributes
{
    using BeachRankings.Data.UnitOfWork;
    using global::App.Code.Blogs;
    using Microsoft.AspNet.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    public class ArticleLinksValidAttribute : ValidationAttribute
    {
        private IBeachRankingsData data;
        private IBlogValidator blogValidator;

        public ArticleLinksValidAttribute()
        {
            this.data = DependencyResolver.Current.GetService<IBeachRankingsData>();
            this.blogValidator = DependencyResolver.Current.GetService<IBlogValidator>();
        }

        public override bool IsValid(object value)
        {
            var authorId = HttpContext.Current.User.Identity.GetUserId();
            var blog = this.data.Users.Find(authorId).Blog;

            if (!this.blogValidator.AllArticleUrlsMatch(blog, value?.ToString()))
            {
                this.ErrorMessage = "The links provided are either invalid, " +
                    "do not belong to your blog, or are duplicates.";

                return false;
            }

            return true;
        }
    }
}