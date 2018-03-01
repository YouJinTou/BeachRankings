namespace App.Code.WaterBodies
{
    using BeachRankings.Models;

    public interface IWaterBodyPermissionChecker
    {
        bool CanEditWaterBody(Country country);

        bool CanAddEditWaterBody(PrimaryDivision primaryDivision, bool adding);

        bool CanAddEditWaterBody(SecondaryDivision secondaryDivision, bool adding);
    }
}
