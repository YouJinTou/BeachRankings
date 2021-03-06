﻿namespace BR.Core.Models
{
    public enum Event
    {
        BeachCreated,
        BeachModified,
        BeachDeleted,

        UserCreatedBeach,
        UserModifiedBeach,
        UserCreated,

        BeachReviewed,
        BeachReviewChanged,
        BeachReviewDeleted,

        ReviewCreated,
        ReviewModified,
        ReviewDeleted,

        UserLeftReview,
        UserModifiedReview,
        UserDeletedReview
    }
}
