namespace BeachRankings.App.ValidationAttributes
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

            var anyFileNull = (files.Any(f => f == null));
            var countExceeded = (files.Length > 3);

            if (anyFileNull || countExceeded)
            {
                return false;
            }

            var filesValid = new bool[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    using (var image = Image.FromStream(files[i].InputStream))
                    {
                        var maxSizeExceeded = (files[i].ContentLength > 1.5 * 1024 * 1024);
                        var fileNameLengthExceeded = (files[i].FileName.Length > 100);
                        var formatCorrect = (
                            image.RawFormat.Equals(ImageFormat.Png) ||
                            image.RawFormat.Equals(ImageFormat.Jpeg) ||
                            image.RawFormat.Equals(ImageFormat.Tiff));

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