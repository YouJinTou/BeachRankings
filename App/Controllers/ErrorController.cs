namespace App.Controllers
{
    using BeachRankings.Data.UnitOfWork;
    using System.Web.Mvc;

    public class ErrorController : BaseController
    {
        public ErrorController(IBeachRankingsData data)
            : base(data)
        {
        }

        public PartialViewResult BadRequest()
        {
            this.Response.StatusCode = 400;

            return this.PartialView("_Error");
        }

        public PartialViewResult PageNotFound()
        {
            this.Response.StatusCode = 404;

            return this.PartialView("_Error");
        }

        public PartialViewResult InternalServerError()
        {
            this.Response.StatusCode = 500;

            return this.PartialView("_Error");
        }
    }
}