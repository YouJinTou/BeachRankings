﻿using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Extensions;
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
                var eventStream = await this.store.GetEventsAsync(id);

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

                var events = await this.store.GetEventsAsync(model.GetId());

                if (events.IsNullOrEmpty())
                {
                    var user = new User(model.Username, model.Email, model.Password);
                    var userCreated = new UserCreated(model.GetId(), 0);

                    await this.store.AddEventsAsync(userCreated.AsEnumerable());

                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create user {model?.Username}.");

                throw;
            }
        }
    }
}
