using BR.Core.Models;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IUsersService
    {
        Task<User> GetUserAsync(string id);

        Task<User> CreateUserAsync(CreateUserModel model);

        Task<User> ModifyUserAsync(ModifyUserModel model);
    }
}
