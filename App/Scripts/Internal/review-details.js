(function () {
    var dragdealersManager = new DragdealersManager();

    dragdealersManager.initializeMeters($('.review-container'));
    genericHelper.setScoreBoxesBackgroundColor();
    beachesHelper.hideEmptyAsideElements();
    reviewsHelper.setReviewVotingVariables();

    $(".light-gallery").lightGallery(); 

    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });

    $('.icon-upvote').on('click', function () {
        reviewsHelper.upvoteReview($(this));
    });

    $('#btn-beach-export-html').on('click', function () {
        genericHelper.exportToHtml('Beaches', 'ExportHtml', $(this).data('html-export-beach'));
    });

    $('.review-export').on('click', function () {
        genericHelper.exportToHtml('Reviews', 'ExportHtml', $(this).data('html-export-review'));
    });

    $('.delete-review-link').on('click', function () {
        return confirm('Delete review?');
    });
})();