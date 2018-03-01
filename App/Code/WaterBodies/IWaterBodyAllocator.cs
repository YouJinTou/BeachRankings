namespace App.Code.WaterBodies
{
    using BeachRankings.Models;

    public interface IWaterBodyAllocator
    {
        void AssignChildrenWaterBodyIds(Country country);

        void AssignChildrenContinentIds(Country country);

        void AssignChildrenWaterBodyIds(PrimaryDivision primaryDivision);

        bool TryAddWaterBody(SecondaryDivision secondaryDivision);

        void AssignChildrenWaterBodyIds(SecondaryDivision secondaryDivision);
    }
}
