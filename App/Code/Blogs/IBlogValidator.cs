using BeachRankings.Models;
using System.Collections.Generic;

namespace App.Code.Blogs
{
    public interface IBlogValidator
    {
        bool AllArticleUrlsMatch(Blog blog, string articleLinks);

        ICollection<string> ValidateBlogger(bool isBlogger, string blogUrl);
    }
}