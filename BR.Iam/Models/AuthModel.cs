using System.ComponentModel.DataAnnotations;

namespace BR.Iam.Models
{
    public class AuthModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string AccessToken { get; set; }
    }
}
