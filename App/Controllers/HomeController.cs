namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Models.ViewModels;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public HomeController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}