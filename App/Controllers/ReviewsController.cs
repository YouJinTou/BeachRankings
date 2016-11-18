namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models;
    using BeachRankings.App.Models.BindingModels;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils;
    using BeachRankings.App.Utils.Extensions;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Linq;
    using System.Web.Mvc;

    public class ReviewsController : BaseController
    {
        public ReviewsController(IBeachRankingsData data)
            : base(data)
        {
        }

        [HttpGet]
        public PartialViewResult Details(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var model = Mapper.Map<Review, DetailedReviewViewModel>(review);            

            return this.PartialView(model);
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

                this.OnErrorSetCriteriaData(model, bindingmodel);
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

            if (this.UserProfile.RecalculateLevel())
            {
                this.Data.Users.SaveChanges();
            }

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            if (this.UserProfile.IsBlogger)
            {
                var blogArticles = BlogsHelper.GetBlogArticles(this.UserProfile.Blogs, bindingModel.ArticleLinks, review.BeachId, review.Id);

                this.Data.BlogArticles.AddMany(blogArticles);
                this.Data.BlogArticles.SaveChanges();
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

                this.OnErrorSetCriteriaData(model, bindingmodel);
            }

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditReviewBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
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

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            reviewedBeach.UpdateScores();

            this.Data.Beaches.SaveChanges();

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

            var author = this.Data.Users.Find(review.AuthorId); // It's possible that a moderator is deleting the review

            this.Data.Reviews.Remove(review);
            this.Data.Reviews.SaveChanges();

            if (author.RecalculateLevel())
            {
                this.Data.Users.SaveChanges();
            }

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

            this.UserProfile.UpvotedReviews.Remove(review);

            this.Data.Reviews.SaveChanges();
            this.Data.Users.SaveChanges();

            return new EmptyResult();
        }

        #region Helpers

        private void OnErrorSetCriteriaData(CriteriaBaseModel viewModel, CriteriaBaseModel bindingModel)
        {
            viewModel.SandQuality = bindingModel.SandQuality;
            viewModel.BeachCleanliness = bindingModel.BeachCleanliness;
            viewModel.BeautifulScenery = bindingModel.BeautifulScenery;
            viewModel.CrowdFree = bindingModel.CrowdFree;
            viewModel.WaterPurity = bindingModel.WaterPurity;
            viewModel.WasteFreeSeabed = bindingModel.WasteFreeSeabed;
            viewModel.FeetFriendlyBottom = bindingModel.FeetFriendlyBottom;
            viewModel.SeaLifeDiversity = bindingModel.SeaLifeDiversity;
            viewModel.CoralReef = bindingModel.CoralReef;
            viewModel.Walking = bindingModel.Walking;
            viewModel.Snorkeling = bindingModel.Snorkeling;
            viewModel.Kayaking = bindingModel.Kayaking;
            viewModel.Camping = bindingModel.Camping;
            viewModel.Infrastructure = bindingModel.Infrastructure;
            viewModel.LongTermStay = bindingModel.LongTermStay;
        }

        #endregion
    }
}