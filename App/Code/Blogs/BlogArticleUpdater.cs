namespace App.Code.Blogs
{
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using System.Linq;

    public class BlogArticleUpdater : IBlogArticleUpdater
    {
        private IBeachRankingsData data;
        private IBlogQueryManager queryManager;

        public BlogArticleUpdater(IBeachRankingsData data, IBlogQueryManager queryManager)
        {
            this.data = data;
            this.queryManager = queryManager;
        }

        public void TryAddUpdateBlogArticles(bool isAdmin, IAddEditBeachModel model)
        {
            if (!isAdmin || string.IsNullOrEmpty(model.ArticleLinks))
            {
                return;
            }

            var newArticles = this.queryManager.GetAdminBlogArticles(model.ArticleLinks, model.Id);
            var existingArticles = this.data.BlogArticles.All().Where(ba => ba.BeachId == model.Id);

            this.data.BlogArticles.RemoveMany(existingArticles);
            this.data.BlogArticles.AddMany(newArticles);
            this.data.BlogArticles.SaveChanges();
        }
    }
}