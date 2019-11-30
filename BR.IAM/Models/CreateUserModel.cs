﻿using System;

namespace BR.Iam.Models
{
    public class CreateUserModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmedPassword { get; set; }

        public void ValidateModel()
        {
            if (!this.Password.Equals(this.ConfirmedPassword))
            {
                throw new ArgumentException("Passwords mismatch.");
            }
        }

        public string GetId()
        {
            return $"{this.Username}|{this.Email}";
        }
    }
}
