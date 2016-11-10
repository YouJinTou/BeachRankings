var dragdealersManager = new DragdealersManager();

(function ($) {
    helper.setScoreBoxesBackgroundColor();

    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });

    $('#beach-details-container').on('click', '.review-expand', function () {
        var $this = $(this);
        var $reviewContainer = $this.closest('.concise-review');
        var $detailsContainer = $reviewContainer.find('.review-details');
        var alreadyLoaded = $reviewContainer.data('already-loaded');
        var expanded = $reviewContainer.data('expanded');

        if (expanded) {
            $detailsContainer.hide();

            $reviewContainer.data('expanded', 'false');

            changeArrowDirection();

            return;
        }

        if (alreadyLoaded) {
            $detailsContainer.show();

            changeArrowDirection();

            return;
        }

        $.ajax({
            url: '/Reviews/Details/',
            type: 'GET',
            data: {
                id: $this.data('review-id')
            },
            success: function (result) {
                $reviewContainer.data('already-loaded', 'true');

                $detailsContainer.append(result);

                $reviewContainer.data('expanded', 'true');

                changeArrowDirection();

                dragdealersManager.initializeMeters();
            }
        });

        function changeArrowDirection() {
            if ($this.hasClass('glyphicon-chevron-down')) {
                $this.removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
            } else {
                $this.removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
            }
        }
    });

    $('#delete-beach-a').on('click', function () {
        $('#delete-beach-form').submit();
    });    
})(jQuery);