﻿namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Linq;
    using System.Web.Mvc;

    public class CountriesController : BaseController
    {
        public CountriesController(IBeachRankingsData data)
            : base(data)
        {
        }
        
        public ActionResult Beaches(int id)
        {
            var country = this.Data.Countries.Find(id);
            var model = Mapper.Map<Country, LocationBeachesViewModel>(country);

            return View(model);
        }

        public JsonResult PrimaryDivisions(int id)
        {
            var primaryDivisions = this.Data.PrimaryDivisions.All().Where(r => r.CountryId == id).Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });

            return this.Json(primaryDivisions, JsonRequestBehavior.AllowGet);
        }
    }
}