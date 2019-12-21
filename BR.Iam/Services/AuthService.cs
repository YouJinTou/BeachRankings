using BR.Core.Abstractions;
using BR.Core.Tools;
using BR.Iam.Abstractions;
using BR.Iam.Models;
using BR.Iam.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.Iam.Services
{
    public class AuthService : IAuthService
    {
        private readonly INoSqlRepository<User> repo;
        private readonly ILogger<AuthService> logger;

        public AuthService(INoSqlRepository<User> repo, ILogger<AuthService> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        public async Task<AuthResult> AuthenticateAsync(AuthModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing auth data.");

                model.ValidateModel();

                var id = User.GetId(model.Email);
                var user = await this.repo.GetAsync(id);
                var passwordsMatch = Hasher.IsValidPassword(
                    user.PasswordHash, model.Password, user.PasswordSalt);
                var result = new AuthResult
                {
                    IsSuccess = passwordsMatch,
                    AccessToken = passwordsMatch ? User.CreateAccessToken() : null,
                    ExpiresAt = passwordsMatch ? DateTime.UtcNow.AddDays(1) : default(DateTime?)
                };
                user.AccessToken = result.AccessToken;
                user.AccessTokenExpiresAt = result.ExpiresAt;

                await this.repo.UpdateAsync(user);

                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to authenticate user {model?.Username}");

                throw;
            }
        }
    }
}
