namespace App.Code.WaterBodies
{
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System.Linq;

    public class WaterBodyPermissionChecker : IWaterBodyPermissionChecker
    {
        IBeachRankingsData data;

        public WaterBodyPermissionChecker(IBeachRankingsData data)
        {
            this.data = data;
        }

        public bool CanAddEditWaterBody(PrimaryDivision primaryDivision, bool adding)
        {
            var country = adding ? this.data.Countries.Find(primaryDivision.CountryId) : primaryDivision.Country;
            var waterBodyAssignedAtCountryLevel = (country.WaterBodyId != null);

            if (waterBodyAssignedAtCountryLevel)
            {
                return false;
            }

            var primaryDivisions = this.data.PrimaryDivisions.All().Where(pd => pd.CountryId == country.Id).ToList();
            var secondaryDivisions = this.data.SecondaryDivisions.All().Where(pd => pd.CountryId == country.Id).ToList();
            var justCreated = (primaryDivisions.All(pd => pd.WaterBodyId == null) && 
                secondaryDivisions.All(sd => sd.WaterBodyId == null));
            var waterBodyAssignedAtPrimaryLevel = justCreated ? true :
                (secondaryDivisions.Count > 0) ? primaryDivisions.All(pd => pd.WaterBodyId != null) : false;

            return waterBodyAssignedAtPrimaryLevel;
        }

        public bool CanEditWaterBody(Country country)
        {
            var noChildren = (country.PrimaryDivisions.Count == 0);

            if (noChildren)
            {
                return true;
            }

            var firstWaterBodyId = country.WaterBodyId;
            var waterBodyAssignedAtPrimaryLevel = country.PrimaryDivisions.Any(pd => pd.WaterBodyId != firstWaterBodyId);

            if (waterBodyAssignedAtPrimaryLevel)
            {
                return false;
            }

            var waterBodyAssignedAtSecondaryLevel = country.SecondaryDivisions.Any(pd => pd.WaterBodyId != firstWaterBodyId);

            return waterBodyAssignedAtSecondaryLevel ? false : true;
        }

        public bool CanAddEditWaterBody(SecondaryDivision secondaryDivision, bool adding)
        {
            var country = adding ? 
                this.data.Countries.Find(secondaryDivision.CountryId) : 
                secondaryDivision.Country;
            var primaryDivision = adding ? 
                this.data.PrimaryDivisions.Find(secondaryDivision.PrimaryDivisionId) : 
                secondaryDivision.PrimaryDivision;
            var waterBodyAssignedAtHigherLevel = (country.WaterBodyId != null || primaryDivision.WaterBodyId != null);

            if (adding)
            {
                var overriding = (secondaryDivision.WaterBodyId != null);

                return waterBodyAssignedAtHigherLevel ?
                    overriding ? false : true :
                    true;
            }

            return waterBodyAssignedAtHigherLevel ? false : true;
        }
    }
}