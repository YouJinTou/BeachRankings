namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.BindingModels;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.App.Utils.Extensions;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
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

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(PostReviewBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (!this.UserProfile.CanRateBeach(bindingModel.BeachId))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = bindingModel.BeachId });
            }

            var review = Mapper.Map<PostReviewBindingModel, Review>(bindingModel);
            review.AuthorId = this.UserProfile.Id;

            this.Data.Reviews.Add(review);
            this.Data.Reviews.SaveChanges();

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            reviewedBeach.UpdateScores();

            this.Data.Beaches.SaveChanges();

            return Json(new { redirectUrl = Url.Action("Details", "Beaches", new { id = bindingModel.BeachId }) });
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var review = this.Data.Reviews.Find(id);

            if (this.User.Identity.CanEditReview(review.AuthorId))
            {
                var model = Mapper.Map<Review, EditReviewViewModel>(review);

                return this.View(model);
            }

            return this.RedirectToAction("Details", "Beaches", new { id = review.BeachId }); // Unauthorized
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditReviewBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var review = this.Data.Reviews.Find(bindingModel.ReviewId);

            if (!this.User.Identity.CanEditReview(bindingModel.AuthorId))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = review.BeachId });
            }

            Mapper.Map(bindingModel, review);

            this.Data.Reviews.SaveChanges();

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            reviewedBeach.UpdateScores();

            this.Data.Beaches.SaveChanges();

            return Json(new { redirectUrl = Url.Action("Details", "Beaches", new { id = reviewedBeach.Id }) });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var review = this.Data.Reviews.Find(id);

            if (!this.User.Identity.CanEditReview(this.UserProfile.Id))
            {
                return this.RedirectToAction("Details", "Beaches", new { id = review.BeachId });
            }

            this.Data.Reviews.Remove(review);
            this.Data.Reviews.SaveChanges();

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Upvote(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var canVote = this.User.Identity.CanVoteForReview(review.AuthorId);
            var alreadyUpvoted = this.User.Identity.ReviewAlreadyUpvoted(id, this.UserProfile.UpvotedReviewIds);

            if (!canVote || (canVote && alreadyUpvoted))
            {
                return new HttpStatusCodeResult(404);
            }

            review.Upvotes += 1;

            this.UserProfile.UpvotedReviewIds.Add(id);

            this.Data.Reviews.SaveChanges();
            this.Data.Users.SaveChanges();

            return new EmptyResult();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Downvote(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var canVote = this.User.Identity.CanVoteForReview(review.AuthorId);
            var hasUpvoted = this.User.Identity.ReviewAlreadyUpvoted(id, this.UserProfile.UpvotedReviewIds);

            if (!canVote || (canVote && !hasUpvoted))
            {
                return new HttpStatusCodeResult(404);
            }

            review.Upvotes -= 1;

            this.UserProfile.UpvotedReviewIds.Remove(id);

            this.Data.Reviews.SaveChanges();
            this.Data.Users.SaveChanges();

            return new EmptyResult();
        }
    }
}