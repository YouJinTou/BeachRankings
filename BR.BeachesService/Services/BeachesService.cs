using AutoMapper;
using BR.BeachesService.Abstractions;
using BR.BeachesService.Events;
using BR.BeachesService.Models;
using BR.Core.Abstractions;
using BR.Core.Events;
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

                if (!stream.IsEmpty())
                {
                    throw new InvalidOperationException("Beach already exists.");
                }

                var userStream = await this.store.GetEventStreamAsync(model.AddedBy);
                var beachCreated = new BeachCreated(beach);
                var userCreatedBeach = new UserCreatedBeach(
                    model.AddedBy, userStream.GetNextOffset(), beach.Id);
                var events = EventStream.CreateStream(beachCreated, userCreatedBeach);

                await this.store.AppendEventStreamAsync(events);

                return beach;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create beach {model?.Name}.");

                throw;
            }
        }

        public async Task<Beach> ModifyBeachAsync(ModifyBeachModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing beach data.");

                var beach = this.mapper.Map<Beach>(model);
                var stream = await this.store.GetEventStreamAsync(Beach.GetId(beach));

                if (stream.IsEmpty())
                {
                    throw new InvalidOperationException("Beach does not exist.");
                }

                if (!stream.IsInitial())
                {
                    throw new InvalidOperationException(
                        $"Modifying to a beach that already exists: {beach.Id}.");
                }

                var userStream = await this.store.GetEventStreamAsync(model.ModifiedBy);
                var beachModified = new BeachModified(beach.Id, stream.GetNextOffset(), model);
                var userModifiedBeach = new UserModifiedBeach(
                    model.ModifiedBy, userStream.GetNextOffset(), beach.Id);
                var events = EventStream.CreateStream(beachModified, userModifiedBeach);

                await this.store.AppendEventStreamAsync(events);

                return beach;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to modify beach {model?.OldName}.");

                throw;
            }
        }
    }
}
