using BR.UsersService.Models;
using System.Threading.Tasks;

namespace BR.UsersService.Abstractions
{
    internal interface IUsersService
    {
        Task CreateUserAsync(CreateUserModel model);
    }
}
