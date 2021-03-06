﻿using BR.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace BR.Core.Models
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
                throw new UserCreationFailedException("Passwords mismatch.");
            }
        }
    }
}
