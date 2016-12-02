(function ($) {
    var expansionInProgress = false;

    hideEmptyAsideElements();
    genericHelper.setScoreBoxesBackgroundColor();
    reviewsHelper.setReviewVotingVariables();
    reviewsHelper.truncateReviews();

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

    function hideEmptyAsideElements() {
        var $container = $('.beach-aside');

        $container.find('.aside-element').each(function () {
            var $this = $(this);
            var captureGroups = /\w+:\s*(\w+)/.exec($this.text());            
            var elementEmpty = (captureGroups === null);

            if (elementEmpty) {
                $this.hide();
            }
        });
    }
})(jQuery);