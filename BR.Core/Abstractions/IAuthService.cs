using BR.Core.Models;
using System.Threading.Tasks;

namespace BR.Core.Abstractions
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginModel model);

        Task<AuthResult> AuthenticateAsync(AuthModel model);
    }
}
