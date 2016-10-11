﻿namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Models.BindingModels;
    using Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class BeachesController : BaseController
    {
        public BeachesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Top()
        {            
            var topBeaches = this.Data.Beaches.All();
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<ConciseBeachViewModel>>(topBeaches);

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var beach = this.Data.Beaches.All().FirstOrDefault(b => b.Id == id);
            var model = Mapper.Map<Beach, DetailedBeachViewModel>(beach);

            model.Reviews.OrderByDescending(r => r.PostedOn);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddBeachBindingModel bindingModel)
        {
            var beach = Mapper.Map<AddBeachBindingModel, Beach>(bindingModel);

            this.Data.Beaches.Add(beach);
            this.Data.Beaches.SaveChanges();

            return RedirectToAction("Rate", "Reviews", new { id = beach.Id });
        }
    }
}