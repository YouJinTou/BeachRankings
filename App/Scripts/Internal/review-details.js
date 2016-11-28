(function () {
    var dragdealersManager = new DragdealersManager();

    dragdealersManager.initializeMeters($('.review-container'));
    genericHelper.setScoreBoxesBackgroundColor();
    reviewsHelper.setReviewVotingVariables();
    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });

    $('.info-icon').on('mouseenter mouseleave', function () {
        $(this).parent().siblings('.custom-popup').toggle();
    });

    $('.icon-upvote').on('click', function () {
        reviewsHelper.upvoteReview($(this));
    });

    $('#btn-beach-export-html').on('click', function () {
        beachesHelper.exportBeachToHtml($(this));
    });

    $('.review-export').on('click', function () {
        reviewsHelper.exportReviewToHtml($(this));
    });
})();