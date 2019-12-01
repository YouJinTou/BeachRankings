using AutoMapper;
using BR.Core.Abstractions;
using BR.Core.Events;
using BR.Core.Tools;
using BR.ReviewsService.Abstractions;
using BR.ReviewsService.Events;
using BR.ReviewsService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BR.ReviewsService.Services
{
    internal class ReviewsService : IReviewsService
    {
        private readonly IEventStore store;
        private readonly IStreamProjector projector;
        private readonly IMapper mapper;
        private readonly ILogger<ReviewsService> logger;

        public ReviewsService(
            IEventStore store, 
            IStreamProjector projector, 
            IMapper mapper, 
            ILogger<ReviewsService> logger)
        {
            this.store = store;
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

                var beachReviewedStream = await this.store.GetEventStreamByTypeAsync(
                    model.BeachId, nameof(BeachReviewed));
                var alreadyReviewed = beachReviewedStream.ContainsEvent<BeachReviewedModel>(
                    m => m.AuthorId == model.AuthorId);

                if (alreadyReviewed)
                {
                    throw new InvalidOperationException(
                        $"Beach {model.BeachId} already reviewed by user {model.AuthorId}.");
                }

                var reviewLeftStream = await this.store.GetEventStreamByTypeAsync(
                    model.AuthorId, nameof(ReviewLeft));
                var review = this.mapper.Map<Review>(model);
                var reviewCreated = new ReviewCreated(review);
                var beachReviwedModel = new BeachReviewedModel(
                    model.BeachId, 
                    beachReviewedStream.NextOffset(nameof(BeachReviewed)), 
                    model.AuthorId, 
                    review.Id);
                var beachReviewed = new BeachReviewed(beachReviwedModel);
                var reviewLeftModel = new ReviewLeftModel(
                    model.AuthorId, 
                    reviewLeftStream.NextOffset(nameof(ReviewLeft)), 
                    review.Id,
                    model.BeachId);
                var reviewLeft = new ReviewLeft(reviewLeftModel);
                var stream = EventStream.CreateStream(reviewCreated, beachReviewed, reviewLeft);

                await this.store.AppendEventStreamAsync(stream);

                return review;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to create review for beach {model?.BeachId}.");

                throw;
            }
        }
    }
}
