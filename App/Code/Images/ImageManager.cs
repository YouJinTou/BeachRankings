namespace App.Code.Images
{
    using App.Code.Beaches;
    using BeachRankings.Data.UnitOfWork;
    using BeachRankings.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;

    public class ImageManager : IImageManager
    {
        private IBeachRankingsData data;
        private IBeachQueryManager queryManager;

        public ImageManager(IBeachRankingsData data, IBeachQueryManager queryManager)
        {
            this.data = data;
            this.queryManager = queryManager;
        }

        public void PersistBeachImages(
            Beach beach, 
            string previousBeachName, 
            IEnumerable<HttpPostedFileBase> images, 
            string authorId)
        {
            if (images == null || images.ElementAt(0) == null)
            {
                return;
            }

            var relativeBeachDir = this.queryManager.GetBeachImagesRelativeDir(beach.Name);
            var beachDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeBeachDir);
            var beachImages = new List<BeachImage>();

            Directory.CreateDirectory(beachDir);

            foreach (var image in images)
            {
                var uniqueName = Guid.NewGuid().ToString() + Path.GetFileName(image.FileName);
                var imagePath = Path.Combine(beachDir, uniqueName);
                var relativeImagePath = Path.Combine("\\", relativeBeachDir, uniqueName);
                var beachImage = new BeachImage
                {
                    AuthorId = authorId,
                    BeachId = beach.Id,
                    Name = uniqueName,
                    Path = relativeImagePath
                };

                image.SaveAs(imagePath);
                beachImages.Add(beachImage);
            }

            this.TryMoveAllImagesFromPreviousLocation(previousBeachName, beach.Name);

            this.TryUpdateOldImagePaths(beachImages, previousBeachName, beach.Name);

            this.data.BeachImages.AddMany(beachImages);
            this.data.BeachImages.SaveChanges();
        }

        public void EraseBeachImagesLocally(string beachName)
        {
            var relativeBeachImagesDir = this.queryManager.GetBeachImagesRelativeDir(beachName);
            var beachDir = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, relativeBeachImagesDir);

            if (Directory.Exists(beachDir))
            {
                var imagesDir = new DirectoryInfo(beachDir);

                imagesDir.Delete(true);
            }
        }

        public void EraseBeachImageLocally(string beachName, string imageName)
        {
            var relativeBeachImagesDir = this.queryManager.GetBeachImagesRelativeDir(beachName);
            var beachDir = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, relativeBeachImagesDir);

            if (!Directory.Exists(beachDir))
            {
                return;
            }

            var imagesDir = new DirectoryInfo(beachDir);
            var image = imagesDir.GetFiles(imageName).FirstOrDefault();

            if (image != null)
            {
                image.Delete();
            }

            if (Directory.GetFiles(beachDir).Length == 0)
            {
                imagesDir.Delete(true);
            }
        }

        private void TryMoveAllImagesFromPreviousLocation(string previousBeachName, string newBeachName)
        {
            if (previousBeachName.ToLower() == newBeachName.ToLower())
            {
                return;
            }

            var oldRelativeBeachDir = this.queryManager.GetBeachImagesRelativeDir(previousBeachName);
            var oldBeachDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oldRelativeBeachDir);

            if (!Directory.Exists(oldBeachDir))
            {
                return;
            }

            var newRelativeBeachDir = this.queryManager.GetBeachImagesRelativeDir(newBeachName);
            var newBeachDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, newRelativeBeachDir);

            if (!Directory.Exists(newBeachDir))
            {
                return;
            }

            var oldLocationImages = new DirectoryInfo(oldBeachDir).GetFiles();

            foreach (var image in oldLocationImages)
            {
                File.Move(image.FullName, Path.Combine(newBeachDir, image.Name));
            }

            Directory.Delete(oldBeachDir, true);
        }

        private void TryUpdateOldImagePaths(
            ICollection<BeachImage> newImages, string previousBeachName, string newBeachName)
        {
            if (previousBeachName.ToLower() == newBeachName.ToLower())
            {
                return;
            }

            var relativeBeachDir = this.queryManager.GetBeachImagesRelativeDir(newBeachName);
            var beachDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeBeachDir);
            var newImageNames = newImages.Select(i => i.Name);
            var imageNamesNeedingPathChange = new DirectoryInfo(beachDir)
                .GetFiles()
                .Select(i => i.Name)
                .Where(n => !newImageNames.Contains(n))
                .ToList();
            var imagesToUpdate = this.data.BeachImages.All()
                .Where(i => imageNamesNeedingPathChange.Any(inn => inn == i.Name))
                .ToList();

            foreach (var image in imagesToUpdate)
            {
                image.Path = Path.Combine("\\", relativeBeachDir, image.Name);
            }
        }
    }
}