using BR.BeachesService.Abstractions;
using BR.BeachesService.Events;
using BR.BeachesService.Models;
using BR.Core.Abstractions;
using BR.Core.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.BeachesService.Services
{
    internal class BeachesService : IBeachesService
    {
        private readonly IEventStore store;
        private readonly ILogger<BeachesService> logger;

        public BeachesService(IEventStore store, ILogger<BeachesService> logger)
        {
            this.store = store;
            this.logger = logger;
        }

        public async Task<Beach> CreateBeachAsync(CreateBeachModel model)
        {
            try
            {
                Validator.ThrowIfNull(model);

                var stream = await this.store.GetEventStreamAsync(model.GetId());
                var beach = new Beach(model.Username, model.Email, model.Password);

                if (stream.IsNullOrEmpty())
                {
                    await this.store.AppendEventAsync(new BeachCreated(beach));

                    return beach;
                }

                throw new InvalidOperationException("Beach already exists.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create beach {model?.Username}.");

                throw;
            }
        }
    }
}
