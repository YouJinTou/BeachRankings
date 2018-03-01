namespace App.Code.Beaches
{
    using BeachRankings.Data.UnitOfWork;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class BeachQueryManager : IBeachQueryManager
    {
        private IBeachRankingsData data;

        public BeachQueryManager(IBeachRankingsData data)
        {
            this.data = data;
        }

        public string GetBeachImagesRelativeDir(string name)
        {
            var formattedBeachName = Regex.Replace(name, @"[^A-Za-z]", string.Empty);
            var relativeBeachDir = Path.Combine("Uploads", "Images", "Beaches", formattedBeachName);

            return relativeBeachDir;
        }

        public int GetBeachWaterBodyId(int countryId, int? primaryDivisionid, int? secondaryDivisionId)
        {
            var secondaryDivision = this.data.SecondaryDivisions.All()
                .Include(sd => sd.WaterBody)
                .FirstOrDefault(sd => sd.Id == secondaryDivisionId);
            var secondaryDivisionHasWaterBody = (secondaryDivision != null && secondaryDivision.WaterBodyId != null);

            if (secondaryDivisionHasWaterBody)
            {
                return (int)secondaryDivision.WaterBodyId;
            }

            var primaryDivision = this.data.PrimaryDivisions.All()
                .Include(pd => pd.WaterBody)
                .FirstOrDefault(pd => pd.Id == primaryDivisionid);
            var primaryDivisionHasWaterBody = (primaryDivision != null && primaryDivision.WaterBodyId != null);

            if (primaryDivisionHasWaterBody)
            {
                return (int)primaryDivision.WaterBodyId;
            }

            var waterBodyId = this.data.Countries.All()
                .Include(c => c.WaterBody)
                .FirstOrDefault(c => c.Id == countryId).WaterBodyId;

            return (int)waterBodyId;
        }
    }
}