namespace App.Controllers
{
    using App.Code.Beaches;
    using App.Code.Blogs;
    using App.Code.Web;
    using AutoMapper;
    using BeachRankings.App.Models;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils;
    using BeachRankings.Extensions;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.Services.Aggregation;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    public class ReviewsController : BaseController
    {
        private IBlogQueryManager blogQueryManager;
        private IWebNameParser webParser;
        private IBeachUpdater beachUpdater;
        private IDataAggregationService aggregationService;

        public ReviewsController(
            IBeachRankingsData data, 
            IBlogQueryManager blogQueryManager, 
            IWebNameParser webParser,
            IBeachUpdater beachUpdater,
            IDataAggregationService aggregationService)
            : base(data)
        {
            this.blogQueryManager = blogQueryManager;
            this.webParser = webParser;
            this.beachUpdater = beachUpdater;
            this.aggregationService = aggregationService;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var model = Mapper.Map<Review, DetailedReviewViewModel>(review);
            model.BeachHead.CrossTable.Rows = Mapper.Map<ICollection<RankContainer>, IEnumerable<CrossTableRowViewModel>>(
                this.aggregationService.CalculateBeachRanks(review.BeachId));

            if (this.User.Identity.IsAuthenticated)
            {
                var upvotedReviewIds = this.Data.UpvotedReviews.All().Select(r => r.AssociatedReviewId).ToList();
                model.ReviewHead.AlreadyUpvoted = this.User.Identity.ReviewAlreadyUpvoted(id, upvotedReviewIds);
                model.BeachHead.UserHasRated = this.UserProfile.Reviews.Any(r => r.BeachId == review.BeachId);
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
            model.BeachHead.CrossTable.Rows = Mapper.Map<ICollection<RankContainer>, IEnumerable<CrossTableRowViewModel>>(
                this.aggregationService.CalculateBeachRanks(beach.Id));
            var isError = (this.TempData["PostReviewViewModel"] != null);

            if (isError)
            {
                var bindingmodel = (PostReviewViewModel)this.TempData["PostReviewViewModel"];
                model.Content = bindingmodel.Content;
                model.ArticleLinks = this.blogQueryManager.GetTrimmedArticleUrl(bindingmodel.ArticleLinks);

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
            if (!this.ModelState.IsValid)
            {
                this.TempData["PostReviewModelStateErrors"] = base.GetModelStateErrors();
                this.TempData["PostReviewViewModel"] = bindingModel;

                return this.RedirectToAction("Post", new { id = bindingModel.BeachHead.Id });
            }

            if (!this.UserProfile.CanRateBeach(bindingModel.BeachHead.Id))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = bindingModel.BeachHead.Id });
            }

            var review = Mapper.Map<PostReviewViewModel, Review>(bindingModel);
            review.AuthorId = this.UserProfile.Id;

            this.Data.Reviews.Add(review);
            this.Data.Reviews.SaveChanges();

            if (this.UserProfile.IsBlogger)
            {
                var articles = this.blogQueryManager.GetUserBlogArticles(
                    this.webParser, this.UserProfile.Blog, bindingModel.ArticleLinks, review.BeachId, review.Id);

                if (articles.Count > 0)
                {
                    this.Data.BlogArticles.AddMany(articles);
                    this.Data.BlogArticles.SaveChanges();
                }
            }

            this.UserProfile.RecalculateLevel(this.Data.ScoreWeights.All().ToList());
            this.Data.Users.SaveChanges();

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            reviewedBeach.UpdateScores();
            this.Data.Beaches.SaveChanges();

            this.beachUpdater.UpdateBeachIndexEntry(reviewedBeach);

            var images = ImageHelper.PersistBeachImages(reviewedBeach, bindingModel.Images, this.UserProfile.Id);

            this.Data.BeachImages.AddMany(images);
            this.Data.BeachImages.SaveChanges();

            return this.RedirectToAction("Details", "Beaches", new { id = bindingModel.BeachHead.Id });
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
            model.BeachHead.UserHasRated = this.UserProfile.Reviews.Any(r => r.BeachId == review.BeachId);
            model.BeachHead.CrossTable.Rows = Mapper.Map<ICollection<RankContainer>, IEnumerable<CrossTableRowViewModel>>(
                this.aggregationService.CalculateBeachRanks(review.BeachId));
            var isError = (this.TempData["EditReviewViewModel"] != null);

            if (isError)
            {
                var bindingmodel = (EditReviewViewModel)this.TempData["EditReviewViewModel"];
                model.Content = bindingmodel.Content;
                model.ArticleLinks = this.blogQueryManager.GetTrimmedArticleUrl(bindingmodel.ArticleLinks);

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
                var newArticles = this.blogQueryManager.GetUserBlogArticles(
                    this.webParser, this.UserProfile.Blog, bindingModel.ArticleLinks, review.BeachId, review.Id);
                var existingArticles = this.Data.BlogArticles.All().Where(ba => ba.ReviewId == review.Id);

                this.Data.BlogArticles.RemoveMany(existingArticles);
                this.Data.BlogArticles.AddMany(newArticles);
                this.Data.BlogArticles.SaveChanges();
            }

            this.UserProfile.RecalculateLevel(this.Data.ScoreWeights.All().ToList());
            this.Data.Users.SaveChanges();

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            reviewedBeach.UpdateScores();
            this.Data.Beaches.SaveChanges();

            this.beachUpdater.UpdateBeachIndexEntry(reviewedBeach);

            return this.RedirectToAction("Details", "Beaches", new { id = reviewedBeach.Id });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var review = this.Data.Reviews.All().Include(r => r.Beach).FirstOrDefault(r => r.Id == id);

            if (!this.User.Identity.CanEditReview(this.UserProfile.Id))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = review.BeachId });
            }

            var existingArticles = this.Data.BlogArticles.All().Where(ba => ba.ReviewId == review.Id);
            var author = this.Data.Users.Find(review.AuthorId); // It's possible that a moderator is deleting the review
            var beach = this.Data.Beaches.Find(review.BeachId);

            if (existingArticles.Count() > 0)
            {
                this.Data.BlogArticles.RemoveMany(existingArticles);
                this.Data.BlogArticles.SaveChanges();
            }

            this.Data.Reviews.Remove(review);
            this.Data.Reviews.SaveChanges();

            author.RecalculateLevel(this.Data.ScoreWeights.All().ToList());
            this.Data.Users.SaveChanges();

            beach.UpdateScores();
            this.Data.Beaches.SaveChanges();

            this.beachUpdater.UpdateBeachIndexEntry(beach);

            return this.RedirectToAction("Details", "Beaches", new { id = review.BeachId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upvote(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var canVote = this.User.Identity.CanVoteForReview(review.AuthorId);
            var alreadyUpvoted = this.Data.UpvotedReviews.UserHasVotedForReview(this.UserProfile.Id, id);

            if (!canVote || (canVote && alreadyUpvoted))
            {
                return new HttpStatusCodeResult(401);
            }

            review.Upvotes += 1;
            review.Author.ThanksReceived += 1;
            var upvotedReview = new UpvotedReview
            {
                AssociatedReviewId = id,
                VoteReceiverId = review.AuthorId,
                UpvotingUserId = this.UserProfile.Id
            };

            this.Data.UpvotedReviews.Add(upvotedReview);

            review.Author.RecalculateLevel(this.Data.ScoreWeights.All().ToList());

            this.Data.Reviews.SaveChanges();
            this.Data.Users.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Downvote(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var canVote = this.User.Identity.CanVoteForReview(review.AuthorId);
            var hasUpvoted = this.Data.UpvotedReviews.UserHasVotedForReview(this.UserProfile.Id, id);

            if (!canVote || (canVote && !hasUpvoted))
            {
                return new HttpStatusCodeResult(401);
            }

            review.Upvotes -= 1;
            review.Author.ThanksReceived -= 1;

            this.Data.UpvotedReviews.Remove(this.Data.UpvotedReviews.GetReviewForUser(this.UserProfile.Id, id));
            review.Author.RecalculateLevel(this.Data.ScoreWeights.All().ToList());

            this.Data.Reviews.SaveChanges();
            this.Data.Users.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        public JsonResult ExportHtml(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var model = Mapper.Map<Review, ExportScoresAsHtmlViewModel>(review);
            var horizontalHtmlResult = this.PartialView(
                @"~\Views\Shared\_ExportScoresHorizontalHtml.cshtml", 
                new List<ExportScoresAsHtmlViewModel> { model }).RenderPartialViewAsString();
            var verticalHtmlResult = this.PartialView(
                @"~\Views\Shared\_ExportScoresVerticalHtml.cshtml", model).RenderPartialViewAsString();
            var htmlResult = horizontalHtmlResult + "@@@" + verticalHtmlResult;

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
        
        #endregion
    }
}