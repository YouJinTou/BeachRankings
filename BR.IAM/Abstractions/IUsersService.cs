using BR.Iam.Models;
using System.Threading.Tasks;

namespace BR.Iam.Abstractions
{
    public interface IUsersService
    {
        Task CreateUserAsync(CreateUserModel model);
    }
}
