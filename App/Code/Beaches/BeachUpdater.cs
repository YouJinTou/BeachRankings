namespace App.Code.Beaches
{
    using App.Code.Blogs;
    using App.Code.Images;
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Data.Entity;
    using System.Linq;

    public class BeachUpdater : IBeachUpdater
    {
        private IBeachRankingsData data;
        private IBeachQueryManager queryManager;
        private IImageManager imageManager;
        private IBlogArticleUpdater blogManager;

        public BeachUpdater(
            IBeachRankingsData data, 
            IBeachQueryManager queryManager, 
            IImageManager imageManager,
            IBlogArticleUpdater blogManager)
        {
            this.data = data;
            this.queryManager = queryManager;
            this.imageManager = imageManager;
            this.blogManager = blogManager;
        }

        public void RemoveChildReferences(Beach beach)
        {
            var articles = beach.BlogArticles.ToList();
            var reviews = beach.Reviews.ToList();
            var images = beach.Images.ToList();

            this.data.BlogArticles.RemoveMany(articles);
            this.data.Reviews.RemoveMany(reviews);
            this.data.BeachImages.RemoveMany(images);
            this.imageManager.EraseBeachImagesLocally(beach.Name);

            this.data.BlogArticles.SaveChanges();
            this.data.Reviews.SaveChanges();
            this.data.BeachImages.SaveChanges();
        }

        public Beach SaveBeach(AddBeachViewModel model, string creatorId)
        {
            this.data.Countries.All()
                .Include(c => c.PrimaryDivisions)
                .Include(c => c.SecondaryDivisions)
                .FirstOrDefault(c => c.Id == model.CountryId);

            var beach = Mapper.Map<AddBeachViewModel, Beach>(model);
            beach.CreatorId = creatorId;
            beach.WaterBodyId = this.queryManager.GetBeachWaterBodyId(
                model.CountryId, model.PrimaryDivisionId, model.SecondaryDivisionId);

            this.data.Beaches.Add(beach);
            beach.SetBeachData();
            this.data.Beaches.SaveChanges();

            this.UpdateIndexEntries(beach);

            return beach;
        }

        public void UpdateBeach(Beach beach, EditBeachViewModel model, bool isAdmin)
        {
            var oldBeach = new Beach();

            Mapper.Map(beach, oldBeach);
            Mapper.Map(model, beach);

            var primaryDivision = this.data.PrimaryDivisions.All()
                .Include(pd => pd.WaterBody)
                .FirstOrDefault(pd => pd.Id == model.PrimaryDivisionId);
            beach.WaterBodyId = this.queryManager
                .GetBeachWaterBodyId(model.CountryId, model.PrimaryDivisionId, model.SecondaryDivisionId);

            this.data.Beaches.SaveChanges();

            beach.SetBeachData();

            this.data.Beaches.SaveChanges();

            this.UpdateIndexEntries(beach, oldBeach);

            this.blogManager.TryAddUpdateBlogArticles(isAdmin, model);
        }

        public void UpdateIndexEntries(Beach beach, Beach oldBeach = null)
        {
            if (oldBeach != null)
            {
                var oldContinent = this.data.Continents.Find(oldBeach.ContinentId);
                var oldCountry = this.data.Countries.Find(oldBeach.CountryId);
                var oldPrimary = this.data.PrimaryDivisions.Find(oldBeach.PrimaryDivisionId);
                var oldSecondary = this.data.SecondaryDivisions.Find(oldBeach.SecondaryDivisionId);
                var oldTertiary = (oldBeach.TertiaryDivisionId == null) ? null : this.data.TertiaryDivisions.Find(oldBeach.TertiaryDivisionId);
                var oldQuaternary = (oldBeach.QuaternaryDivisionId == null) ? null : this.data.QuaternaryDivisions.Find(oldBeach.QuaternaryDivisionId);
                var oldWaterBody = this.data.WaterBodies.Find(oldBeach.WaterBodyId);

                this.data.Beaches.AddUpdateIndexEntry(oldBeach);
                this.data.Continents.AddUpdateIndexEntry(oldContinent);
                this.data.Countries.AddUpdateIndexEntry(oldCountry);
                this.data.PrimaryDivisions.AddUpdateIndexEntry(oldPrimary);
                this.data.SecondaryDivisions.AddUpdateIndexEntry(oldSecondary);
                this.data.TertiaryDivisions.AddUpdateIndexEntry(oldTertiary);
                this.data.QuaternaryDivisions.AddUpdateIndexEntry(oldQuaternary);
                this.data.WaterBodies.AddUpdateIndexEntry(oldWaterBody);
            }

            var continent = this.data.Continents.Find(beach.ContinentId);
            var country = this.data.Countries.Find(beach.CountryId);
            var primary = this.data.PrimaryDivisions.Find(beach.PrimaryDivisionId);
            var secondary = this.data.SecondaryDivisions.Find(beach.SecondaryDivisionId);
            var tertiary = (beach.TertiaryDivisionId == null) ? null : this.data.TertiaryDivisions.Find(beach.TertiaryDivisionId);
            var quaternary = (beach.QuaternaryDivisionId == null) ? null : this.data.QuaternaryDivisions.Find(beach.QuaternaryDivisionId);
            var waterBodyId = (secondary == null) ?
                (primary == null) ? country.WaterBodyId : primary.WaterBodyId
                : secondary.WaterBodyId;
            var waterBody = this.data.WaterBodies.Find(waterBodyId);

            this.data.Beaches.AddUpdateIndexEntry(beach);
            this.data.Continents.AddUpdateIndexEntry(continent);
            this.data.Countries.AddUpdateIndexEntry(country);
            this.data.PrimaryDivisions.AddUpdateIndexEntry(primary);
            this.data.SecondaryDivisions.AddUpdateIndexEntry(secondary);
            this.data.TertiaryDivisions.AddUpdateIndexEntry(tertiary);
            this.data.QuaternaryDivisions.AddUpdateIndexEntry(quaternary);
            this.data.WaterBodies.AddUpdateIndexEntry(waterBody);
        }

        public void UpdateBeachIndexEntry(Beach beach)
        {
            this.data.Beaches.AddUpdateIndexEntry(beach);
        }

        public void UpdateBeachScores()
        {
            var beaches = this.data.Beaches.All();

            foreach (var beach in beaches)
            {
                beach.UpdateScores();
            }

            this.data.Beaches.SaveChanges();

            foreach (var beach in beaches)
            {
                this.UpdateBeachIndexEntry(beach);
            }
        }
    }
}