namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Helpers;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using System;
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
            review.TotalScore = ReviewsHelper.GetTotalReviewScore(review);
            review.AuthorId = this.User.Identity.GetUserId();
            review.PostedOn = DateTime.Now;

            this.Data.Reviews.Add(review);
            this.Data.Reviews.SaveChanges();

            return Json(new { redirectUrl = Url.Action("Details", "Beaches", new { id = bindingModel.BeachId }) });
        }
    }
}