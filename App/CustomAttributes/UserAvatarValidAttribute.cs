namespace BeachRankings.App.CustomAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Web;

    public class UserAvatarValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;

            if (file == null)
            {
                return true;
            }

            try
            {
                using (var image = Image.FromStream(file.InputStream))
                {
                    var maxSizeExceeded = (file.ContentLength > 250 * 1024);
                    var fileNameLengthExceeded = (file.FileName.Length > 100);
                    var formatCorrect = (image.RawFormat.Equals(ImageFormat.Png) || image.RawFormat.Equals(ImageFormat.Jpeg));
                    var dimensionsCorrect = (image.Width <= 300 && image.Height <= 300);

                    if (maxSizeExceeded || fileNameLengthExceeded || !formatCorrect || !dimensionsCorrect)
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}