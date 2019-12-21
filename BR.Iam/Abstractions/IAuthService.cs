﻿using BR.Iam.Models;
using System.Threading.Tasks;

namespace BR.Iam.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginModel model);
    }
}
