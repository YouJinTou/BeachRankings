(function ($) {
    var infiniteScroller = new InfiniteScroller('beach-details-reviews-container', 'beach');
    var expansionInProgress = false;

    setElementsResponsive();
    beachesHelper.hideEmptyAsideElements();
    genericHelper.setScoreBoxesBackgroundColor();
    reviewsHelper.setReviewVotingVariables();
    reviewsHelper.truncateReviews();
    infiniteScroller.setInifiniteScroll();

    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });   

    $('#beach-details-container').on('click', '.icon-upvote', function () {
        reviewsHelper.upvoteReview($(this));
    });

    $('#btn-beach-export-html').on('click', function () {
        beachesHelper.exportBeachToHtml($(this));
    });

    $('#beach-details-container').on('click', '.review-export', function () {
        reviewsHelper.exportReviewToHtml($(this));
    });

    $('#delete-beach-span').on('click', function () {
        var toDelete = confirm("Are you sure you want to delete this beach?");

        if (toDelete) {
            $('#delete-beach-form').submit();
        }
    });

    $('#beach-details-container').on('click', '.delete-review-link', function () {
        return confirm('Delete review?');
    });

    function setElementsResponsive() {
        var viewportWidth = genericHelper.getViewportWidth();

        if (viewportWidth <= 768) {
            var aside = $('.beach-aside-box').insertAfter('.beach-head');
        }
    }
})(jQuery);