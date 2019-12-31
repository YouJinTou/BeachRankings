using AutoMapper;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Tools;
using BR.ReviewsService.Abstractions;
using BR.ReviewsService.Events;
using BR.ReviewsService.Factories;
using BR.ReviewsService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.ReviewsService.Services
{
    internal class ReviewsService : IReviewsService
    {
        private readonly IEventStore store;
        private readonly IEventBus bus;
        private readonly IStreamProjector projector;
        private readonly IMapper mapper;
        private readonly ILogger<ReviewsService> logger;

        public ReviewsService(
            IEventStore store,
            IEventBus bus,
            IStreamProjector projector, 
            IMapper mapper, 
            ILogger<ReviewsService> logger)
        {
            this.store = store;
            this.bus = bus;
            this.projector = projector;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Review> GetReviewAsync(string id)
        {
            try
            {
                Validator.ThrowIfNullOrWhiteSpace(id, "No review ID.");

                var stream = await this.store.GetEventStreamAsync(id);
                var aggregate = this.projector.GetSnapshot(stream);

                return (Review)aggregate;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to get review {id}.");

                throw;
            }
        }

        public async Task<Review> CreateReviewAsync(CreateReviewModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing review data.");

                var beachReviewedStream = await this.store.GetEventStreamAsync(
                    model.BeachId, nameof(BeachReviewed));
                var alreadyReviewed = beachReviewedStream.ContainsEvent<BeachReviewedModel>(
                    m => m.UserId == model.UserId);

                if (alreadyReviewed)
                {
                    throw new InvalidOperationException(
                        $"Beach {model.BeachId} already reviewed by user {model.UserId}.");
                }

                var reviewLeftStream = await this.store.GetEventStreamAsync(
                    model.UserId, nameof(UserLeftReview));
                var review = this.mapper.Map<Review>(model);
                var set = EventSetFactory.CreateSet(review, beachReviewedStream, reviewLeftStream);

                await this.bus.PublishEventStreamAsync(set.ToStream());

                return review;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create review for beach {model?.BeachId}.");

                throw;
            }
        }

        public async Task<Review> ModifyReviewAsync(ModifyReviewModel model)
        {
            try
            {
                Validator.ThrowIfNull(model, "Missing review data.");

                var reviewStream = await this.store.GetEventStreamAsync(model.Id.ToString());
                var userStream = await this.store.GetEventStreamAsync(model.UserId);
                var beachStream = await this.store.GetEventStreamAsync(model.BeachId);
                var reviewModified = new ReviewModified(model, reviewStream.GetNextOffset());
                var userModifiedReviewModel = this.mapper.Map<UserModifiedReviewModel>(model);
                userModifiedReviewModel.Offset = userStream.GetNextOffset();
                var userModifiedReview = new UserModifiedReview(userModifiedReviewModel);
                var beachReviewChangedModel = this.mapper.Map<BeachReviewChangedModel>(model);
                beachReviewChangedModel.Offset = beachStream.GetNextOffset();
                var beachReviewChanged = new BeachReviewChanged(beachReviewChangedModel);
                var stream = EventStream.CreateStream(
                    reviewModified, userModifiedReview, beachReviewChanged);

                await this.bus.PublishEventStreamAsync(stream);

                var review = this.mapper.Map<Review>(model);

                return review;
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    ex, $"Failed to modify review {model?.Id} for beach {model?.BeachId}.");

                throw;
            }
        }
    }
}
