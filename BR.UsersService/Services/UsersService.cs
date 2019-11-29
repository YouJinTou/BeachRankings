using BR.Core.Abstractions;
using BR.Core.Tools;
using BR.UsersService.Abstractions;
using BR.UsersService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.UsersService.Services
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

        public async Task CreateUserAsync(CreateUserModel model)
        {
            try
            {
                Validator.ThrowIfNull(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create user {model?.Username}.");

                throw;
            }
        }
    }
}
