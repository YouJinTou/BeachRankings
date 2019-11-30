using BR.Iam.Models;
using System.Threading.Tasks;

namespace BR.Iam.Abstractions
{
    internal interface IUsersService
    {
        Task CreateUserAsync(CreateUserModel model);
    }
}
