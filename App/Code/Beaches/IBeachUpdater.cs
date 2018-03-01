namespace App.Code.Beaches
{
    using BeachRankings.App.Models.ViewModels;
    using BeachRankings.Models;

    public interface IBeachUpdater
    {
        void RemoveChildReferences(Beach beach);

        Beach SaveBeach(AddBeachViewModel model, string creatorID);

        void UpdateBeach(Beach beach, EditBeachViewModel model, bool isAdmin);

        void UpdateIndexEntries(Beach beach, Beach oldBeach = null);

        void UpdateBeachIndexEntry(Beach beach);

        void UpdateBeachScores();
    }
}
