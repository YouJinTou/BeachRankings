namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using Models.ViewModels;
    using System.Collections.Generic;
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
            var model = Mapper.Map<IEnumerable<Beach>, IEnumerable<BeachSummaryViewModel>>(topBeaches);

            return View(model);
        }
    }
}