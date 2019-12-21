using BR.Core.Abstractions;
using BR.Core.Tools;
using BR.Iam.Abstractions;
using BR.Iam.Events;
using BR.Iam.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Iam.Services
{
    internal class UsersService : IUsersService
    {
        private readonly INoSqlRepository<User> repo;
        private readonly IEventStore store;
        private readonly IStreamProjector projector;
        private readonly ILogger<UsersService> logger;

        public UsersService(
            INoSqlRepository<User> repo, 
            IEventStore store, 
            IStreamProjector projector, 
            ILogger<UsersService> logger)
        {
            this.repo = repo;
            this.store = store;
            this.projector = projector;
            this.logger = logger;
        }

        public async Task<User> GetUserAsync(string id)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(id, "No user ID.");

                var stream = await this.store.GetEventStreamAsync(id);
                var aggregate = this.projector.GetSnapshot(stream);

                return (User)aggregate;
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

                try
                {
                    var existingUser = await this.repo.GetAsync(model.GetId());

                    throw new InvalidOperationException("User already exists.");
                }
                catch (KeyNotFoundException)
                {
                }

                var user = new User(model.Username, model.Email, model.Password);

                await this.repo.AddAsync(user);

                await this.store.AppendEventAsync(new UserCreated(user));

                return user;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create user {model?.Username}.");

                throw;
            }
        }

        public async Task<User> ModifyUserAsync(ModifyUserModel model)
        {
            try
            {
                Validator.ThrowIfNull(model);

                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to modify user {model?.Id}.");

                throw;
            }
        }
    }
}
