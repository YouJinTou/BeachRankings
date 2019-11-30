using BR.Core.Abstractions;
using BR.Core.Extensions;
using BR.Core.Tools;
using BR.Iam.Abstractions;
using BR.Iam.Events;
using BR.Iam.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.Iam.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IEventStore store;
        private readonly ILogger<UsersService> logger;

        public UsersService(IEventStore store, ILogger<UsersService> logger)
        {
            this.store = store;
            this.logger = logger;
        }

        public async Task<User> GetUserAsync(string id)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(id, "No user ID.");

                var eventStream = await this.store.GetEventStreamAsync(id);

                return null;
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

                var events = await this.store.GetEventStreamAsync(model.GetId());
                var user = new User(model.Username, model.Email, model.Password);

                if (events.IsNullOrEmpty())
                {
                    await this.store.AppendEventAsync(new UserCreated(user));

                    return user;
                }

                await this.store.AppendEventAsync(new CreateUserFailed(user));

                throw new InvalidOperationException("User already exists.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create user {model?.Username}.");

                throw;
            }
        }
    }
}
