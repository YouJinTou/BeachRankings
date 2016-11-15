(function ($) {
    var dragdealersManager = new DragdealersManager();
    var expansionInProgress = false;
    var votingInProgress = false;

    hideEmptyAsideElements();
    helper.setScoreBoxesBackgroundColor();
    helper.toggleReviewThumbs();

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
            adjustScreen($this, true);

            return;
        }

        if (alreadyLoaded) {
            $detailsContainer.show(400);            
            $reviewContainer.data('expanded', 'true');
            changeArrowDirection();
            adjustScreen($this, false);

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
                $reviewContainer.data('expanded', 'true');

                $detailsContainer.append(result);
                dragdealersManager.initializeMeters($detailsContainer);

                changeArrowDirection();
                adjustScreen($this, false);
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

        function adjustScreen($arrow, collapsing) {
            var offset = collapsing ? -($(window).height() / 3) : 0;

            $('html, body').animate({
                scrollTop: $arrow.offset().top + offset
            }, 750);
        }
    });

    $('#beach-details-container').on('click', '.icon-upvote', function () {
        if (votingInProgress) {
            return;
        }
        
        var $this = $(this);
        var isUpvote = $this.hasClass('glyphicon-thumbs-up');
        var url = isUpvote ? '/Reviews/Upvote/' : '/Reviews/Downvote';

        $.ajax({
            url: url,
            type: 'POST',
            data: {
                id: $this.data('review-id')
            },
            beforeSend: function () {
                votingInProgress = true;
            },
            success: function (result) {
                changeThumbsDirection();
            }, complete: function () {
                votingInProgress = false;
            }
        });

        function changeThumbsDirection() {
            if ($this.hasClass('glyphicon-thumbs-up')) {
                $this.removeClass('glyphicon-thumbs-up').addClass('glyphicon-thumbs-down');
            } else {
                $this.removeClass('glyphicon-thumbs-down').addClass('glyphicon-thumbs-up');
            }
        }
    });

    $('#delete-beach-span').on('click', function () {
        $('#delete-beach-form').submit();
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