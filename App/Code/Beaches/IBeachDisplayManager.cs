namespace App.Code.Beaches
{
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Models;

    public interface IBeachDisplayManager
    {
        EditBeachViewModel GetEditBeachViewModel(Beach beach);

        string GetFilteredBeachesTitle(int continentId, int countryId, int waterBodyId, int criterionId);

        string GetControllerNameFromId(int continentId, int countryId, int waterBodyId);

        int GetPlaceId(int continentId, int countryId, int waterBodyId);
    }
}
