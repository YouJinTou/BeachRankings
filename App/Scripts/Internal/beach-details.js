var dragdealersManager = new DragdealersManager();

(function ($) {
    var expansionInProgress = false;

    helper.setScoreBoxesBackgroundColor();

    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });

    $('#beach-details-container').on('click', '.review-expand', function () {
        if (expansionInProgress) {
            return;
        }

        var $this = $(this);
        var $reviewContainer = $this.closest('.concise-review');
        var $detailsContainer = $reviewContainer.find('.review-details');
        var alreadyLoaded = ($reviewContainer.data('already-loaded') === 'true');
        var expanded = ($reviewContainer.data('expanded') === 'true');

        if (expanded) {
            $detailsContainer.hide(400);

            $reviewContainer.data('expanded', 'false');

            changeArrowDirection();

            scrollScreen($this, true);

            return;
        }

        if (alreadyLoaded) {
            $detailsContainer.show(400);
            
            $reviewContainer.data('expanded', 'true');

            changeArrowDirection();

            scrollScreen($this, false);

            return;
        }

        $.ajax({
            url: '/Reviews/Details/',
            type: 'GET',
            data: {
                id: $this.data('review-id')
            },
            beforeSend: function () {
                expansionInProgress = true;
            },
            success: function (result) {
                $reviewContainer.data('already-loaded', 'true');

                $detailsContainer.append(result);
                dragdealersManager.initializeMeters($detailsContainer);

                $reviewContainer.data('expanded', 'true');

                changeArrowDirection();

                scrollScreen($this, false);
            },complete: function () {
                expansionInProgress = false;
            }
        });

        function changeArrowDirection() {
            if ($this.hasClass('glyphicon-chevron-down')) {
                $this.removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
            } else {
                $this.removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
            }
        }

        function scrollScreen($arrow, collapsing) {
            var margin = collapsing ? -($(window).height() / 3) : 0;

            $('html, body').animate({
                scrollTop: $arrow.offset().top + margin
            }, 500);
        }
    });

    $('#delete-beach-a').on('click', function () {
        $('#delete-beach-form').submit();
    });    
})(jQuery);