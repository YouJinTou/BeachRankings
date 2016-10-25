namespace App.Controllers
{
    using AutoMapper;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.App.Models.ViewModels;
    using System;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public class BeachesController : BaseController
    {
        public BeachesController(IBeachRankingsData data)
            : base(data)
        {
        }

        public ActionResult Details(int id)
        {
            var beach = this.Data.Beaches.All().FirstOrDefault(b => b.Id == id);
            var model = Mapper.Map<Beach, DetailedBeachViewModel>(beach);

            model.Reviews.OrderByDescending(r => r.PostedOn);

            return this.View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            var countries = this.Data.Countries.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            var model = new AddBeachViewModel()
            {
                Countries = countries
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddBeachViewModel bindingModel)
        {
            var beachNameUnique = !this.Data.Beaches.All().Any(
                    b => b.Region.Id == bindingModel.RegionId && 
                    b.Area.Id == bindingModel.AreaId && 
                    b.Name.ToLower() == bindingModel.Name.ToLower());

            if (!beachNameUnique)
            {
                this.ModelState.AddModelError(string.Empty, "A beach with this name already exists.");
            }

            if (!this.ModelState.IsValid)
            {
                bindingModel.Countries = this.Data.Countries.All().Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

                return this.View(bindingModel);
            }

            this.Data.Countries.All().Include(c => c.Regions).Include(c => c.Areas).FirstOrDefault(c => c.Id == bindingModel.CountryId);
            var region = this.Data.Regions.All().Include(r => r.WaterBody).FirstOrDefault(r => r.Id == bindingModel.RegionId);
            var beach = Mapper.Map<AddBeachViewModel, Beach>(bindingModel);
            beach.WaterBodyId = region.WaterBodyId;
            
            this.Data.Beaches.Add(beach);
            beach.SetBeachData();
            this.Data.Beaches.SaveChanges();

            this.Data.Beaches.AddBeachToIndex(beach);

            if (bindingModel.Images != null)
            {
                var formattedBeachName = Regex.Replace(beach.Name, @"[^A-Za-z]", string.Empty);
                var beachDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads", "Images", "Beaches", formattedBeachName);

                Directory.CreateDirectory(beachDir);

                foreach (var image in bindingModel.Images)
                {
                    var uniqueName = Guid.NewGuid().ToString() + Path.GetFileName(image.FileName);
                    var imagePath = Path.Combine(beachDir, uniqueName);
                    var beachPhoto = new BeachImage()
                    {
                        AuthorId = this.UserProfile.Id,
                        BeachId = beach.Id,
                        Name = uniqueName,
                        Path = imagePath
                    };

                    image.SaveAs(imagePath);
                    this.Data.BeachImages.Add(beachPhoto);
                }                
                
                this.Data.BeachImages.SaveChanges();
            }

            return this.RedirectToAction("Post", "Reviews", new { id = beach.Id });
        }
    }
}