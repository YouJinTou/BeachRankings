namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.Models.Enums;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Services.Aggregation;
    using BeachRankings.Extensions;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
   
    using System;

    public class WaterBodiesController : BasePlacesController
    {
        public WaterBodiesController(IBeachRankingsData data, IDataAggregationService aggregationService)
            : base(data, aggregationService)
        {
        }

        public ActionResult Beaches(int id, int page = 0, int pageSize = 10)
        {
            var waterBody = this.Data.WaterBodies.Find(id);
            var model = Mapper.Map<WaterBody, PlaceBeachesViewModel>(waterBody);
            model.Controller = "WaterBodies";
            model.Beaches = model.Beaches.OrderByDescending(b => b.TotalScore).Skip(page * pageSize).Take(pageSize);

            model.Beaches.Select(b => { b.UserHasRated = base.UserHasRated(b); return b; }).ToList();

            return this.View("_PlaceBeaches", model);
        }

        public ActionResult Statistics(int id)
        {
            var waterBody = this.Data.WaterBodies.All().Include(wb => wb.Beaches).FirstOrDefault(wb => wb.Id == id);
            var beaches = waterBody.Beaches.Where(b => b.TotalScore != null).OrderByDescending(b => b.TotalScore);
            var model = new StatisticsViewModel()
            {
                Id = id,
                Controller = "WaterBodies",
                Name = waterBody.Name,
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches),
                TotalBeachesCount = beaches.Count(),
            };

            return this.View("_StatisticsPartial", model);
        }

        public ActionResult FilteredStatistics(int id, string filterType)
        {
            var waterBody = this.Data.WaterBodies.All().Include(wb => wb.Beaches).FirstOrDefault(wb => wb.Id == id);
            var beaches = waterBody.Beaches.Where(b => b.TotalScore != null).OrderByDescending(b => b.TotalScore);
            var model = new StatisticsViewModel()
            {
                Id = id,
                Controller = "WaterBodies",
                Name = ((BeachFilterType)Enum.Parse(typeof(BeachFilterType), filterType)).GetDescription() + " " + waterBody.Name,
                Rows = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachRowViewModel>>(beaches),
                TotalBeachesCount = beaches.Count(),
                FilterType = filterType
            };

            if (this.Request.IsAjaxRequest())
            {
                return this.Json(this.PartialView("_StatisticsPartial", model)
                    .RenderPartialViewAsString(), JsonRequestBehavior.AllowGet);
            }

            return this.View("_StatisticsPartial", model);
        }
    }
}