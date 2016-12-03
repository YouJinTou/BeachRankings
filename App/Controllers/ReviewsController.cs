namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils;
    using BeachRankings.App.Utils.Extensions;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class ReviewsController : BaseController
    {
        public ReviewsController(IBeachRankingsData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var model = Mapper.Map<Review, DetailedReviewViewModel>(review);

            if (this.User.Identity.IsAuthenticated)
            {
                var upvotedReviewIds = this.UserProfile.UpvotedReviews.Select(r => r.Id).ToList();
                model.UserHasRated = this.UserProfile.Reviews.Any(r => r.BeachId == review.BeachId);
                model.AlreadyUpvoted = this.User.Identity.ReviewAlreadyUpvoted(id, upvotedReviewIds);
            }          

            return this.View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Post(int id)
        {
            if (!this.UserProfile.CanRateBeach(id))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = id });
            }

            var beach = this.Data.Beaches.Find(id);
            var model = Mapper.Map<Beach, PostReviewViewModel>(beach);
            model.IsBlogger = this.UserProfile.IsBlogger;
            var isError = (this.TempData["PostReviewViewModel"] != null);

            if (isError)
            {
                var bindingmodel = (PostReviewViewModel)this.TempData["PostReviewViewModel"];
                model.Content = bindingmodel.Content;
                model.ArticleLinks = BlogHelper.TrimArticleUrl(bindingmodel.ArticleLinks);

                this.OnErrorSetCriteriaData(model, bindingmodel);
                base.AddModelStateErrors((ICollection<string>)this.TempData["PostReviewModelStateErrors"]);
            }

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(PostReviewViewModel bindingModel)
        {
            this.ValidateArticleLinks(bindingModel.ArticleLinks);

            if (!this.ModelState.IsValid)
            {
                this.TempData["PostReviewModelStateErrors"] = base.GetModelStateErrors();
                this.TempData["PostReviewViewModel"] = bindingModel;

                return this.RedirectToAction("Post", new { id = bindingModel.BeachId });
            }

            if (!this.UserProfile.CanRateBeach(bindingModel.BeachId))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = bindingModel.BeachId });
            }

            var review = Mapper.Map<PostReviewViewModel, Review>(bindingModel);
            review.AuthorId = this.UserProfile.Id;

            this.Data.Reviews.Add(review);
            this.Data.Reviews.SaveChanges();

            this.UserProfile.RecalculateLevel();
            this.Data.Users.SaveChanges();

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            if (this.UserProfile.IsBlogger)
            {
                var articles = BlogHelper.GetBlogArticles(this.UserProfile.Blog, bindingModel.ArticleLinks, review.BeachId, review.Id);

                if (articles.Count > 0)
                {
                    this.Data.BlogArticles.AddMany(articles);
                    this.Data.BlogArticles.SaveChanges();
                }               
            }

            reviewedBeach.UpdateScores();
            this.Data.Beaches.SaveChanges();

            return this.RedirectToAction("Details", "Beaches", new { id = bindingModel.BeachId });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var review = this.Data.Reviews.Find(id);

            if (!this.User.Identity.CanEditReview(review.AuthorId))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = review.BeachId });
            }

            var model = Mapper.Map<Review, EditReviewViewModel>(review);
            model.IsBlogger = this.UserProfile.IsBlogger;
            var isError = (this.TempData["EditReviewViewModel"] != null);

            if (isError)
            {
                var bindingmodel = (EditReviewViewModel)this.TempData["EditReviewViewModel"];
                model.Content = bindingmodel.Content;
                model.ArticleLinks = BlogHelper.TrimArticleUrl(bindingmodel.ArticleLinks);

                this.OnErrorSetCriteriaData(model, bindingmodel);
                base.AddModelStateErrors((ICollection<string>)this.TempData["EditReviewModelStateErrors"]);
            }

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditReviewViewModel bindingModel)
        {
            this.ValidateArticleLinks(bindingModel.ArticleLinks);

            if (!this.ModelState.IsValid)
            {
                this.TempData["EditReviewModelStateErrors"] = base.GetModelStateErrors();
                this.TempData["EditReviewViewModel"] = bindingModel;

                return this.RedirectToAction("Edit", new { id = bindingModel.Id });
            }

            var review = this.Data.Reviews.Find(bindingModel.Id);

            if (!this.User.Identity.CanEditReview(review.AuthorId))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = review.BeachId });
            }

            Mapper.Map(bindingModel, review);

            this.Data.Reviews.SaveChanges();

            if (this.UserProfile.IsBlogger)
            {
                var newArticles = BlogHelper.GetBlogArticles(this.UserProfile.Blog, bindingModel.ArticleLinks, review.BeachId, review.Id);
                var existingArticles = this.Data.BlogArticles.All().Where(ba => ba.ReviewId == review.Id);

                this.Data.BlogArticles.RemoveMany(existingArticles);
                this.Data.BlogArticles.AddMany(newArticles);
                this.Data.BlogArticles.SaveChanges();
            }

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            reviewedBeach.UpdateScores();
            this.Data.Beaches.SaveChanges();

            this.UserProfile.RecalculateLevel();
            this.Data.Users.SaveChanges();

            return this.RedirectToAction("Details", "Beaches", new { id = reviewedBeach.Id });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var review = this.Data.Reviews.Find(id);

            if (!this.User.Identity.CanEditReview(this.UserProfile.Id))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = review.BeachId });
            }

            var existingArticles = this.Data.BlogArticles.All().Where(ba => ba.ReviewId == review.Id);
            var author = this.Data.Users.Find(review.AuthorId); // It's possible that a moderator is deleting the review

            this.Data.BlogArticles.RemoveMany(existingArticles);
            this.Data.BlogArticles.SaveChanges();

            this.Data.Reviews.Remove(review);
            this.Data.Reviews.SaveChanges();

            author.RecalculateLevel();
            this.Data.Users.SaveChanges();

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upvote(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var upvotedReviewIds = this.UserProfile.UpvotedReviews.Select(r => r.Id).ToList();
            var canVote = this.User.Identity.CanVoteForReview(review.AuthorId);
            var alreadyUpvoted = this.User.Identity.ReviewAlreadyUpvoted(id, upvotedReviewIds);

            if (!canVote || (canVote && alreadyUpvoted))
            {
                return new HttpStatusCodeResult(401);
            }

            review.Upvotes += 1;
            review.Author.ThanksReceived += 1;

            this.UserProfile.UpvotedReviews.Add(review);

            this.Data.Reviews.SaveChanges();
            this.Data.Users.SaveChanges();

            return new EmptyResult();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Downvote(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var upvotedReviewIds = this.UserProfile.UpvotedReviews.Select(r => r.Id).ToList();
            var canVote = this.User.Identity.CanVoteForReview(review.AuthorId);
            var hasUpvoted = this.User.Identity.ReviewAlreadyUpvoted(id, upvotedReviewIds);

            if (!canVote || (canVote && !hasUpvoted))
            {
                return new HttpStatusCodeResult(401);
            }

            review.Upvotes -= 1;
            review.Author.ThanksReceived -= 1;

            this.UserProfile.UpvotedReviews.Remove(review);

            this.Data.Reviews.SaveChanges();
            this.Data.Users.SaveChanges();

            return new EmptyResult();
        }

        public JsonResult ExportHtml(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var model = Mapper.Map<Review, ExportScoresAsHtmlViewModel>(review);
            var htmlResult = this.PartialView(@"~\Views\Shared\_ExportScoresHtml.cshtml", model).RenderPartialViewAsString();

            return this.Json(htmlResult, JsonRequestBehavior.AllowGet);
        }

        #region Helpers

        private void OnErrorSetCriteriaData(CriteriaBaseModel viewModel, CriteriaBaseModel bindingModel)
        {
            viewModel.SandQuality = bindingModel.SandQuality;
            viewModel.BeachCleanliness = bindingModel.BeachCleanliness;
            viewModel.BeautifulScenery = bindingModel.BeautifulScenery;
            viewModel.CrowdFree = bindingModel.CrowdFree;
            viewModel.Infrastructure = bindingModel.Infrastructure;
            viewModel.WaterVisibility = bindingModel.WaterVisibility;
            viewModel.LitterFree = bindingModel.LitterFree;
            viewModel.FeetFriendlyBottom = bindingModel.FeetFriendlyBottom;
            viewModel.SeaLifeDiversity = bindingModel.SeaLifeDiversity;
            viewModel.CoralReef = bindingModel.CoralReef;
            viewModel.Snorkeling = bindingModel.Snorkeling;
            viewModel.Kayaking = bindingModel.Kayaking;
            viewModel.Walking = bindingModel.Walking;
            viewModel.Camping = bindingModel.Camping;
            viewModel.LongTermStay = bindingModel.LongTermStay;
        }

        private void ValidateArticleLinks(string articleLinks)
        {
            if (!this.UserProfile.IsBlogger)
            {
                return;
            }

            if (!BlogHelper.AllArticleUrlsMatched(this.UserProfile.Blog, articleLinks))
            {
                this.ModelState.AddModelError(string.Empty, "The links provided are either invalid, do not belong to your blog, or are duplicates.");
            }
        }

        #endregion
    }
}