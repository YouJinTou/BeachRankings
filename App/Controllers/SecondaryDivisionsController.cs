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

        public PartialViewResult Beaches(int id)
        {
            var beaches = this.Data.SecondaryDivisions.All()
                .Include(sd => sd.PrimaryDivision.WaterBody)
                .Include(sd => sd.TertiaryDivisions)
                .Include(sd => sd.QuaternaryDivisions)
                .FirstOrDefault(sd => sd.Id == id)
                .Beaches
                .Where(b => b.Reviews.Count > 0);
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