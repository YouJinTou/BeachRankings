using AutoMapper;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Models;
using BR.Core.Tools;
using BR.ReviewsService.Abstractions;
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
                    model.BeachId, Event.BeachReviewed.ToString());
                var alreadyReviewed = beachReviewedStream.ContainsEvent<BeachReviewedModel>(
                    m => m.UserId == model.UserId);

                if (alreadyReviewed)
                {
                    throw new InvalidOperationException(
                        $"Beach {model.BeachId} already reviewed by user {model.UserId}.");
                }

                var review = this.mapper.Map<Review>(model);
                var reviewCreated = new AppEvent(
                review.Id.ToString(), review, Event.ReviewCreated.ToString());
                var beachReviwedModel = new BeachReviewedModel(
                    review.BeachId, review.UserId, review.Id);
                var beachReviewed = new AppEvent(
                    review.BeachId, beachReviwedModel, Event.BeachReviewed.ToString());
                var userLeftReviewModel = new UserLeftReviewModel(
                    review.UserId, review.Id, review.BeachId);
                var userLeftReview = new AppEvent(
                    review.UserId, userLeftReviewModel, Event.UserLeftReview.ToString());
                var stream = EventStream.CreateStream(
                    reviewCreated, beachReviewed, userLeftReview);

                await this.bus.PublishEventStreamAsync(stream);

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

                var reviewModified = new AppEvent(
                    model.Id.ToString(), model, Event.ReviewModified.ToString());
                var userModifiedReviewModel = this.mapper.Map<UserModifiedReviewModel>(model);
                var userModifiedReview = new AppEvent(
                    model.UserId, 
                    userModifiedReviewModel, 
                    Event.UserModifiedReview.ToString());
                var beachReviewChangedModel = this.mapper.Map<BeachReviewChangedModel>(model);
                var beachReviewChanged = new AppEvent(
                    model.BeachId, 
                    beachReviewChangedModel, 
                    Event.BeachReviewChanged.ToString());
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
