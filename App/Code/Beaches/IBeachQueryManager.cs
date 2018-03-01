namespace App.Code.Beaches
{
    public interface IBeachQueryManager
    {
        string GetBeachImagesRelativeDir(string name);

        int GetBeachWaterBodyId(int countryId, int? primaryDivisionid, int? secondaryDivisionId);
    }
}
