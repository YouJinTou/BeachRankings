using BR.Iam.Models;
using System;
using System.Threading.Tasks;

namespace BR.Iam.Abstractions
{
    public interface IUsersService
    {
        Task<User> GetUserAsync(Guid id);

        Task<User> CreateUserAsync(CreateUserModel model);
    }
}
