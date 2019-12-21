﻿using System;

namespace BR.Iam.Models
{
    public class LoginResult
    {
        public bool IsSuccess { get; set; }

        public string AccessToken { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}