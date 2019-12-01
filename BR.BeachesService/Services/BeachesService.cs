using AutoMapper;
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
        private readonly IStreamProjector projector;
        private readonly IMapper mapper;
        private readonly ILogger<BeachesService> logger;

        public BeachesService(
            IEventStore store, 
            IStreamProjector projector, 
            IMapper mapper, 
            ILogger<BeachesService> logger)
        {
            this.store = store;
            this.projector = projector;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Beach> GetBeachAsync(string id)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(id, "No beach ID.");

                var stream = await this.store.GetEventStreamAsync(id);
                var aggregate = this.projector.GetSnapshot(stream);

                return (Beach)aggregate;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get beach {id}.");

                throw;
            }
        }

        public async Task<Beach> CreateBeachAsync(CreateBeachModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing beach data.");

                var beach = this.mapper.Map<Beach>(model);
                var stream = await this.store.GetEventStreamAsync(Beach.GetId(beach));

                if (stream.IsEmpty())
                {
                    await this.store.AppendEventAsync(new BeachCreated(beach));

                    return beach;
                }

                throw new InvalidOperationException("Beach already exists.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create beach {model?.Name}.");

                throw;
            }
        }
    }
}
