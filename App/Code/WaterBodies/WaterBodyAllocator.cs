namespace App.Code.WaterBodies
{
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System;

    public class WaterBodyAllocator : IWaterBodyAllocator
    {
        IBeachRankingsData data;

        public WaterBodyAllocator(IBeachRankingsData data)
        {
            this.data = data;
        }

        public void AssignChildrenWaterBodyIds(Country country)
        {
            foreach (var beach in country.Beaches)
            {
                beach.WaterBodyId = (int)country.WaterBodyId;
            }

            foreach (var primaryDivision in country.PrimaryDivisions)
            {
                primaryDivision.WaterBodyId = country.WaterBodyId;
            }

            foreach (var secondaryDivision in country.SecondaryDivisions)
            {
                secondaryDivision.WaterBodyId = country.WaterBodyId;
            }
        }

        public void AssignChildrenContinentIds(Country country)
        {
            foreach (var beach in country.Beaches)
            {
                beach.ContinentId = (int)country.ContinentId;
            }

            foreach (var primaryDivision in country.PrimaryDivisions)
            {
                primaryDivision.ContinentId = country.ContinentId;
            }

            foreach (var secondaryDivision in country.SecondaryDivisions)
            {
                secondaryDivision.ContinentId = country.ContinentId;
            }

            foreach (var tertiaryDivision in country.TertiaryDivisions)
            {
                tertiaryDivision.ContinentId = country.ContinentId;
            }

            foreach (var quaternaryDivision in country.QuaternaryDivisions)
            {
                quaternaryDivision.ContinentId = country.ContinentId;
            }
        }

        public void AssignChildrenWaterBodyIds(PrimaryDivision primaryDivision)
        {
            foreach (var beach in primaryDivision.Beaches)
            {
                beach.WaterBodyId = (int)primaryDivision.WaterBodyId;
            }

            foreach (var secondaryDivision in primaryDivision.SecondaryDivisions)
            {
                secondaryDivision.WaterBodyId = primaryDivision.WaterBodyId;
            }
        }

        public bool TryAddWaterBody(SecondaryDivision secondaryDivision)
        {
            var country = this.data.Countries.Find(secondaryDivision.CountryId);
            var primaryDivision = this.data.PrimaryDivisions.Find(secondaryDivision.PrimaryDivisionId);
            var waterBodyId = (country.WaterBodyId != null) ? country.WaterBodyId : primaryDivision.WaterBodyId;

            if (waterBodyId == null)
            {
                return false;
            }

            secondaryDivision.WaterBodyId = waterBodyId;

            return true;
        }

        public void AssignChildrenWaterBodyIds(SecondaryDivision secondaryDivision)
        {
            foreach (var beach in secondaryDivision.Beaches)
            {
                beach.WaterBodyId = (int)secondaryDivision.WaterBodyId;
            }
        }
    }
}