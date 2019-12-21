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

        public async Task<LoginResult> LoginAsync(LoginModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing login data.");

                model.ValidateModel();

                var id = User.GetId(model.Email);
                var user = await this.repo.GetAsync(id);
                var passwordsMatch = Hasher.IsValidPassword(
                    user.PasswordHash, model.Password, user.PasswordSalt);
                var result = new LoginResult
                {
                    IsSuccess = passwordsMatch,
                    AccessToken = passwordsMatch ? User.CreateAccessToken() : null,
                    ExpiresAt = passwordsMatch ? DateTime.UtcNow.AddDays(1) : default(DateTime?),
                    Username = passwordsMatch ? user.Username : null,
                    Email = passwordsMatch ? user.Email : null
                };
                user.AccessToken = result.AccessToken;
                user.AccessTokenExpiresAt = result.ExpiresAt;

                await this.repo.UpdateAsync(user);

                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to login user {model?.Username}");

                return new LoginResult { IsSuccess = false };
            }
        }

        public async Task<AuthResult> AuthenticateAsync(AuthModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing auth data.");

                var id = User.GetId(model.Email);
                var user = await this.repo.GetAsync(id);
                var usernamesMatch = user.Username == model.Username;
                var emailsMatch = user.Email == model.Email;
                var accessTokensMatch =
                    !string.IsNullOrWhiteSpace(user.AccessToken)
                    && user.AccessToken == model.AccessToken;
                var expirationDateValid = 
                    user.AccessTokenExpiresAt.HasValue 
                    && user.AccessTokenExpiresAt > DateTime.UtcNow;
                var result = new AuthResult
                {
                    IsSuccess = 
                        usernamesMatch 
                        && emailsMatch 
                        && accessTokensMatch 
                        && expirationDateValid
                };

                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to authenticate user {model?.Username}");

                return new AuthResult { IsSuccess = false };
            }
        }
    }
}
