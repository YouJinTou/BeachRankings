using AutoMapper;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Models;
using BR.Core.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Core.Services
{
    internal class BeachesService : IBeachesService
    {
        private readonly IEventStore store;
        private readonly IEventBus bus;
        private readonly IStreamProjector projector;
        private readonly IMapper mapper;
        private readonly ILogger<BeachesService> logger;

        public BeachesService(
            IEventStore store,
            IEventBus bus,
            IStreamProjector projector,
            IMapper mapper,
            ILogger<BeachesService> logger)
        {
            this.store = store;
            this.bus = bus;
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

        public async Task<IEnumerable<Beach>> GetBeachesAsync(IEnumerable<string> ids)
        {
            try
            {
                Validator.ThrowIfNull(ids, "No beach IDs.");

                var streams = await this.store.GetEventStreamsAsync(ids);
                var aggregates = streams
                    .Select(s => this.projector.GetSnapshot(s)).Cast<Beach>().ToList();

                return aggregates;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get beaches {string.Join(" ", ids)}.");

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
                var beachCreated = new AppEvent(beach.Id, beach, Event.BeachCreated.ToString());
                var userCreatedBeach = new AppEvent(
                    model.AddedBy, beach.Id, Event.UserCreatedBeach.ToString());
                var events = EventStream.CreateStream(beachCreated, userCreatedBeach);

                await this.bus.PublishEventStreamAsync(events);

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

                var userStream = await this.store.GetEventStreamAsync(model.ModifiedBy);
                var beachModified = new AppEvent(beach.Id, model, Event.BeachModified.ToString());
                var userModifiedBeach = new AppEvent(
                    model.ModifiedBy, beach.Id, Event.UserModifiedBeach.ToString());
                var events = EventStream.CreateStream(beachModified, userModifiedBeach);

                await this.bus.PublishEventStreamAsync(events);

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
