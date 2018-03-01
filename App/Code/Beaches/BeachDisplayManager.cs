namespace App.Code.Beaches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using BeachRankings.Models.Enums;
    using BeachRankings.Services.Initializers;

    public class BeachDisplayManager : IBeachDisplayManager
    {
        private IBeachRankingsData data;

        public BeachDisplayManager(IBeachRankingsData data)
        {
            this.data = data;
        }

        public string GetControllerNameFromId(int continentId, int countryId, int waterBodyId)
        {
            return (continentId == 0) ?
                (countryId == 0) ? "WaterBodies" : "Countries" :
                "Continents";
        }

        public EditBeachViewModel GetEditBeachViewModel(Beach beach)
        {
            var model = Mapper.Map<Beach, EditBeachViewModel>(beach);
            model.Countries = this.data.Countries.All().Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString(),
                Selected = (c.Id == model.CountryId)
            });
            model.PrimaryDivisions = this.data.PrimaryDivisions.All()
                .Where(pd => pd.CountryId == beach.CountryId)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = (c.Id == model.PrimaryDivisionId)
                });
            model.SecondaryDivisions = this.data.SecondaryDivisions.All()
                .Where(sd => sd.PrimaryDivisionId == beach.PrimaryDivisionId)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = (c.Id == model.SecondaryDivisionId)
                });
            model.TertiaryDivisions = this.data.TertiaryDivisions.All()
                .Where(td => td.SecondaryDivisionId == beach.SecondaryDivisionId)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = (c.Id == model.TertiaryDivisionId)
                });
            model.QuaternaryDivisions = this.data.QuaternaryDivisions.All()
                .Where(qd => qd.TertiaryDivisionId == beach.TertiaryDivisionId)
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = (c.Id == model.QuaternaryDivisionId)
                });

            return model;
        }

        public string GetFilteredBeachesTitle(int continentId, int countryId, int waterBodyId, int criterionId)
        {
            var intro = "Top 25 Beaches in ";
            var continent = (continentId == 0) ? string.Empty : GeoInitializer.Continents[continentId];
            var country = (countryId == 0) ? string.Empty : GeoInitializer.Countries[countryId];
            var waterBody = (waterBodyId == 0) ? string.Empty : ("the " + GeoInitializer.WaterBodies[waterBodyId]);
            var place = TrimBeachesTitle((continent + ", " + country + waterBody));
            var criterionExist = (criterionId > 0 && criterionId <= 15);

            if (!criterionExist)
            {
                return intro + place;
            }

            var criterion = (Criterion)criterionId;

            switch (criterion)
            {
                case Criterion.Camping:
                    return intro + place + " for Camping";
                case Criterion.LongTermStay:
                    return intro + place + " for Long-term Stay";
                case Criterion.Snorkeling:
                    return intro + place + " for Snorkeling";
                default:
                    return intro + place;
            }
        }

        public int GetPlaceId(int continentId, int countryId, int waterBodyId)
        {
            return (continentId == 0) ?
                (countryId == 0) ? waterBodyId : countryId :
                continentId;
        }

        private string TrimBeachesTitle(string title)
        {
            var commonSeas = new HashSet<string> { "Caribbean Sea", "Mediterranean Sea" };
            var result = title.Trim();
            var landMissing = (result[0] == ',' || result[result.Length - 1] == ',');

            if (landMissing)
            {
                result = result.Trim(new char[] { ',', ' ' });
            }

            if (commonSeas.Any(s => result.Contains(s)))
            {
                result = result.Replace(" Sea", string.Empty);
            }

            return result;
        }
    }
}