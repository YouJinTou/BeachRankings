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
        public ActionResult Details(int id)
        {
            var review = this.Data.Reviews.Find(id);
            var model = Mapper.Map<Review, DetailedReviewViewModel>(review);            

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

            return this.RedirectToAction("Details", new { id = id }); // Unauthorized
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

            if (!this.User.Identity.CanEditReview(bindingModel.AuthorId))
            {
                return this.RedirectToAction("Details", new { id = bindingModel.ReviewId });
            }

            var review = this.Data.Reviews.Find(bindingModel.ReviewId);

            Mapper.Map(bindingModel, review);

            this.Data.Reviews.SaveChanges();

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            reviewedBeach.UpdateScores();

            this.Data.Beaches.SaveChanges();

            return Json(new { redirectUrl = Url.Action("Details", "Reviews", new { id = bindingModel.ReviewId }) });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            if (!this.User.Identity.CanEditReview(this.UserProfile.Id))
            {
                return this.RedirectToAction("Details", new { id = id });
            }

            var review = this.Data.Reviews.Find(id);

            this.Data.Reviews.Remove(review);
            this.Data.Reviews.SaveChanges();

            return this.RedirectToAction("Index", "Home");
        }
    }
}