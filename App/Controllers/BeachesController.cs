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
            this.ValidateBindingModel(bindingModel);

            if (!this.ModelState.IsValid)
            {
                bindingModel.Countries = this.Data.Countries.All().Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

                return this.View(bindingModel);
            }

            var beach = this.SaveBeach(bindingModel);

            this.SaveBeachImages(beach, bindingModel);            

            return this.RedirectToAction("Post", "Reviews", new { id = beach.Id });
        }

        private void ValidateBindingModel(AddBeachViewModel model)
        {
            var beachNameUnique = !this.Data.Beaches.All().Any(
                b => b.PrimaryDivisionId == model.PrimaryDivisionId &&
                b.SecondaryDivisionId == model.SecondaryDivisionId &&
                b.Name.ToLower() == model.Name.ToLower());
            var tertiaryDivisionsExist = (this.Data.SecondaryDivisions.Find(model.SecondaryDivisionId).TertiaryDivisions.Count > 0);
            var tertiaryIdMissing = (tertiaryDivisionsExist && model.TertiaryDivisionId == null);
            var quaternaryIdMissing = (tertiaryDivisionsExist && !tertiaryIdMissing) ? 
                ((this.Data.TertiaryDivisions.Find(model.TertiaryDivisionId)).QuaternaryDivisions.Count > 0) &&
                (model.QuaternaryDivisionId == null) : 
                tertiaryDivisionsExist ? true : false;      

            if (tertiaryIdMissing || quaternaryIdMissing)
            {
                this.ModelState.AddModelError(string.Empty, "All location fields are required.");
            }

            if (!beachNameUnique)
            {
                this.ModelState.AddModelError(string.Empty, "A beach with this name already exists.");
            }
        }

        private Beach SaveBeach(AddBeachViewModel model)
        {
            // OPT?
            this.Data.Countries.All().Include(c => c.PrimaryDivisions).Include(c => c.SecondaryDivisions).FirstOrDefault(c => c.Id == model.CountryId);
            var country = this.Data.Countries.Find(model.CountryId);
            var primary = this.Data.PrimaryDivisions.All().Include(p => p.WaterBody).FirstOrDefault(p => p.Id == model.PrimaryDivisionId);
            var secondary = this.Data.SecondaryDivisions.Find(model.SecondaryDivisionId);
            var tertiary = (model.TertiaryDivisionId == null) ? null : this.Data.TertiaryDivisions.Find(model.TertiaryDivisionId);
            var quaternary = (model.QuaternaryDivisionId == null) ? null : this.Data.QuaternaryDivisions.Find(model.QuaternaryDivisionId);
            var waterBody = this.Data.WaterBodies.Find(primary.WaterBodyId);
            var beach = Mapper.Map<AddBeachViewModel, Beach>(model);
            beach.WaterBodyId = primary.WaterBodyId;

            this.Data.Beaches.Add(beach);
            beach.SetBeachData();
            this.Data.Beaches.SaveChanges();

            this.Data.Beaches.AddUpdateIndexEntry(beach);
            this.Data.Countries.AddUpdateIndexEntry(country);
            this.Data.PrimaryDivisions.AddUpdateIndexEntry(primary);
            this.Data.SecondaryDivisions.AddUpdateIndexEntry(secondary);
            this.Data.TertiaryDivisions.AddUpdateIndexEntry(tertiary);
            this.Data.QuaternaryDivisions.AddUpdateIndexEntry(quaternary);
            this.Data.WaterBodies.AddUpdateIndexEntry(waterBody);

            return beach;
        }

        private void SaveBeachImages(Beach beach, AddBeachViewModel bindingModel)
        {
            if (bindingModel.Images == null || bindingModel.Images.ElementAt(0) == null)
            {
                return;
            }

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
    }
}