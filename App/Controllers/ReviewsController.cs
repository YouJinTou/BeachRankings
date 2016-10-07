namespace App.Controllers
{
    using BeachRankings.Data.UnitOfWork;
    using System.Web.Mvc;

    public class ReviewsController : BaseController
    {
        public ReviewsController(IBeachRankingsData data)
            : base(data)
        {
        }

        [Authorize]
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            return View();
        }
    }
}