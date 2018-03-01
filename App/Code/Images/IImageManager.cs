namespace App.Code.Images
{
    using BeachRankings.Models;
    using System.Collections.Generic;
    using System.Web;

    public interface IImageManager
    {
        void PersistBeachImages(
            Beach beach, 
            string previousBeachName, 
            IEnumerable<HttpPostedFileBase> images, 
            string authorId);

        void EraseBeachImagesLocally(string beachName);

        void EraseBeachImageLocally(string beachName, string imageName);
    }
}
