namespace BeachRankings.App.CustomAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Web;

    public class ImagesValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var files = value as HttpPostedFileBase[];
            var nothingUploaded = (files.Length == 1 && files[0] == null);

            if (nothingUploaded)
            {
                return true;
            }

            if (files.Any(f => f == null))
            {
                return false;
            }

            var filesValid = new bool[files.Length];
            var maxTotalSize = 3.5 * 1024 * 1024;
            var runningSize = 0;

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    using (var image = Image.FromStream(files[i].InputStream))
                    {
                        runningSize += files[i].ContentLength;
                        var fileNameLengthExceeded = (files[i].FileName.Length > 100);
                        var formatCorrect = (image.RawFormat.Equals(ImageFormat.Png) || image.RawFormat.Equals(ImageFormat.Jpeg));
                        var maxSizeExceeded = (runningSize > maxTotalSize);

                        if (maxSizeExceeded || fileNameLengthExceeded || !formatCorrect)
                        {
                            break;
                        }
                    }
                }
                catch
                {
                    break;
                }

                filesValid[i] = true;
            }           

            return filesValid.All(fileValid => fileValid);
        }
    }
}