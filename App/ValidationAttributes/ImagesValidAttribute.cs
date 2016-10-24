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

            if (files.Length == 1 && files[0] == null)
            {
                return true;
            }

            if (files.Any(f => f == null))
            {
                this.ErrorMessage = "Failed to upload images. Verify their size and format.";

                return false;
            }

            var filesValid = new bool[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].ContentLength > 1.5 * 1024 * 1024)
                {
                    this.ErrorMessage = "Images cannot be larger than 1.5 MB.";

                    break;
                }

                if (files[i].FileName.Length > 50)
                {
                    this.ErrorMessage = "Image names cannot be longer than 50 characters.";

                    break;
                }

                try
                {
                    using (var image = Image.FromStream(files[i].InputStream))
                    {
                        var formatCorrect = (
                            image.RawFormat.Equals(ImageFormat.Png) ||
                            image.RawFormat.Equals(ImageFormat.Jpeg) ||
                            image.RawFormat.Equals(ImageFormat.Tiff));

                        if (!formatCorrect)
                        {
                            this.ErrorMessage = "Invalid iamge format. Allowed formats: PNG, JPG, Tiff.";

                            break;
                        }
                    }
                }
                catch
                {
                    this.ErrorMessage = "Failed to upload images. Please verify their size and format.";

                    break;
                }

                filesValid[i] = true;
            }           

            return filesValid.All(fileValid => fileValid);
        }
    }
}