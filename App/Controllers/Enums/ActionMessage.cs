using System.ComponentModel;

namespace App.Controllers.Enums
{
    public enum ActionMessage
    {
        AddPhoneSuccess,
        [Description("Your password has been changed.")]
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        [Description("Your avatar has been changed.")]
        ChangeAvatarSuccess,
        [Description("Your avatar has been deleted.")]
        DeleteAvatarSuccess,
        [Description("Failed to upload avatar. Verify its size, dimensions, and format.")]
        UploadAvatarError,
        [Description("We couldn't process the blog URL provided.")]
        BlogUrlInvalid,
        [Description("A blog with this URL already exists.")]
        BlogUrlExists,
        [Description("An error has occurred. Please try again.")]
        Error
    }
}