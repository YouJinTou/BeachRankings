namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using System.Web.Mvc;

    public class ReviewsController : BaseController
    {
        public ReviewsController(IBeachRankingsData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpGet]
        public ActionResult Post()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Post(PostReviewBindingModel bindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            var review = Mapper.Map<PostReviewBindingModel, Review>(bindingModel);
            review.AuthorId = this.User.Identity.GetUserId();

            this.Data.Reviews.Add(review);
            this.Data.Reviews.SaveChanges();

            return Json(new { redirectUrl = Url.Action("Index", "Home") });
        }
    }
}