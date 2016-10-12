namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using System.Web.Mvc;

    public class ReviewsController : BaseController
    {
        public ReviewsController(IBeachRankingsData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpGet]
        public ActionResult Rate(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rate(PostReviewBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            var review = Mapper.Map<PostReviewBindingModel, Review>(bindingModel);
            review.AuthorId = this.User.Identity.GetUserId();

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

            if (review == null)
            {
                return View(); // Error
            }

            var currentUserId = this.UserProfile.Id;

            if (review.AuthorId == currentUserId)
            {
                var model = Mapper.Map<Review, EditReviewViewModel>(review);

                return View(model);
            }

            return View(); // Unauthorized
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditReviewBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            var review = this.Data.Reviews.Find(bindingModel.ReviewId);

            Mapper.Map(bindingModel, review);

            this.Data.Reviews.SaveChanges();

            var reviewedBeach = this.Data.Beaches.Find(review.BeachId);

            reviewedBeach.UpdateScores();

            this.Data.Beaches.SaveChanges();

            return Json(new { redirectUrl = Url.Action("Details", "Reviews", new { id = bindingModel.ReviewId }) });
        }
    }
}