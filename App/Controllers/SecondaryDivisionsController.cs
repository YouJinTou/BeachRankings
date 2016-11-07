namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class SecondaryDivisionsController : BaseController
    {
        public SecondaryDivisionsController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Beaches(int id)
        {
            var secondaryDivision = this.Data.SecondaryDivisions.Find(id);
            var model = Mapper.Map<SecondaryDivision, LocationBeachesViewModel>(secondaryDivision);

            return this.View("LocationBeaches", model);
        }

        public PartialViewResult BeachesPartial(int id)
        {
            var beaches = this.Data.SecondaryDivisions.All()
                .Include(sd => sd.PrimaryDivision.WaterBody)
                .Include(sd => sd.TertiaryDivisions)
                .Include(sd => sd.QuaternaryDivisions)
                .FirstOrDefault(sd => sd.Id == id)
                .Beaches
                .Where(b => b.TotalScore != null);
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachTableRowViewModel>>(beaches);

            return this.PartialView(model);
        }

        public JsonResult TertiaryDivisions(int id)
        {
            var tertiaryDivisions = this.Data.TertiaryDivisions.All().Where(td => td.SecondaryDivisionId == id).Select(td => new SelectListItem()
            {
                Text = td.Name,
                Value = td.Id.ToString()
            });

            return this.Json(tertiaryDivisions, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> BeachNames(int id, string term)
        {
            var beachNames = await this.Data.Beaches.All()
                .Where(b => b.SecondaryDivisionId == id && b.Name.StartsWith(term))
                .Select(b => b.Name)
                .ToListAsync();

            return this.Json(beachNames, JsonRequestBehavior.AllowGet);
        }
    }
}