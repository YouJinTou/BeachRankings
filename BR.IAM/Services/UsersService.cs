using BR.Core.Abstractions;
using BR.Core.Models;
using BR.Core.Tools;
using BR.Iam.Abstractions;
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
        private readonly IEventBus bus;
        private readonly ILogger<UsersService> logger;

        public UsersService(
            INoSqlRepository<User> repo,
            IEventBus bus,
            ILogger<UsersService> logger)
        {
            this.repo = repo;
            this.bus = bus;
            this.logger = logger;
        }

        public async Task<User> GetUserAsync(string id)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(id, "No user ID.");

                var user = await this.repo.GetAsync(id);

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

                try
                {
                    var id = User.GetId(model.Email);
                    var existingUser = await this.repo.GetAsync(id);

                    throw new InvalidOperationException("User already exists.");
                }
                catch (KeyNotFoundException)
                {
                }

                var user = new User(model.Username, model.Email, model.Password);

                await this.repo.AddAsync(user);

                await this.bus.PublishEventAsync(
                    new AppEvent(user.Id, user, Event.UserCreated.ToString()));

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

                await Task.CompletedTask;

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
