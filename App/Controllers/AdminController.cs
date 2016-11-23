namespace App.Controllers
{
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using System.Linq;
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public AdminController(IBeachRankingsData data)
            : base(data)
        {
        }

        [HttpGet]
        public ActionResult Restructure()
        {
            var countries = this.Data.Countries.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            var model = new RestructureViewModel() { Countries = countries };

            return this.View(model);
        }
    }
}