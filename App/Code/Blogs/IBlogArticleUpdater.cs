namespace App.Code.Blogs
{
    using BeachRankings.App.Models.ViewModels;

    public interface IBlogArticleUpdater
    {
        void TryAddUpdateBlogArticles(bool isAdmin, IAddEditBeachModel model);
    }
}
