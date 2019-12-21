using BR.Iam.Models;
using System.Threading.Tasks;

namespace BR.Iam.Abstractions
{
    public interface IAuthService
    {
        Task<LoginResult> LoginAsync(LoginModel model);

        Task<AuthResult> AuthenticateAsync(AuthModel model);
    }
}
