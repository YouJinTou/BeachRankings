using BR.Iam.Tools;
using System;
using System.ComponentModel.DataAnnotations;

namespace BR.Iam.Models
{
    public class CreateUserModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmedPassword { get; set; }

        public void ValidateModel()
        {
            if (!this.Password.Equals(this.ConfirmedPassword))
            {
                throw new ArgumentException("Passwords mismatch.");
            }
        }
    }
}
