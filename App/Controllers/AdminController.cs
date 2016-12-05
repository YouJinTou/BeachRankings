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
            var continents = this.Data.Continents.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            var countries = this.Data.Countries.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            var waterBodies = this.Data.WaterBodies.All().Select(wb => new SelectListItem()
            {
                Text = wb.Name,
                Value = wb.Id.ToString()
            });
            var model = new RestructureViewModel()
            {
                Continents = continents,
                Countries = countries,
                WaterBodies = waterBodies
            };

            if (this.TempData["ValidationError"] != null)
            {
                this.ModelState.AddModelError(string.Empty, this.TempData["ValidationError"].ToString());
            }

            return this.View(model);
        }
    }
}