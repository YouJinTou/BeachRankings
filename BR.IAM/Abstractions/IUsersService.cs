﻿using BR.Iam.Models;
using System.Threading.Tasks;

namespace BR.Iam.Abstractions
{
    public interface IUsersService
    {
        Task<User> GetUserAsync(string id);

        Task<User> CreateUserAsync(CreateUserModel model);

        Task<User> ModifyUserAsync(ModifyUserModel model);
    }
}