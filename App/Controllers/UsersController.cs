namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.Models.Interfaces;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Services.Search;
    using BeachRankings.Services.Search.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class UsersController : BaseController
    {
        public UsersController(IBeachRankingsData data)
            : base(data)
        {
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            return this.View();
        }
    }
}