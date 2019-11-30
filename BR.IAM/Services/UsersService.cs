using BR.Core.Abstractions;
using BR.Core.Tools;
using BR.Iam.Abstractions;
using BR.Iam.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.Iam.Services
{
    internal class UsersService : IUsersService
    {
        private readonly INoSqlRepository<User> repo;
        private readonly IEventStore store;
        private readonly ILogger<UsersService> logger;

        public UsersService(
            INoSqlRepository<User> repo, IEventStore store, ILogger<UsersService> logger)
        {
            this.repo = repo;
            this.store = store;
            this.logger = logger;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            try
            {
                var user = await this.repo.GetAsync(id.ToString());

                return user;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get user {id}.");

                throw;
            }
        }

        public async Task<User> CreateUserAsync(CreateUserModel model)
        {
            try
            {
                Validator.ThrowIfNull(model);

                model.ValidateModel();

                var user = new User(model.Username, model.Email, model.Password);

                await this.repo.AddAsync(user);

                return user;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create user {model?.Username}.");

                throw;
            }
        }
    }
}
