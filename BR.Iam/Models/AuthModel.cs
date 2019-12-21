
using BR.Core.Tools;

namespace BR.Iam.Models
{
    public class AuthModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Password { get; set; }

        public void ValidateModel()
        {
            Validator.ThrowIfAllNull(this.Username, this.Email);
        }
    }
}
