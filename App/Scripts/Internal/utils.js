var GenericHelper = function () {
    function setScoreBoxesBackgroundColor(html) {
        if (!html) {
            $('.beach-score-box .vertical-center').each(function () {
                var $this = $(this);

                $this.parent().css('background-color', getScoreBoxBackgroundColor($this.text()));
            });
        } else {
            $(html).find('.beach-score-box .vertical-center').each(function () {
                var $this = $(this);

                $this.parent().css('background-color', getScoreBoxBackgroundColor($this.text()));
            });
        }
    }

    function openModalPopup(html, value) {
        var $mainPopup = $('[data-popup="main"');
        var $htmlCase = $mainPopup.find('[data-html-case]');
        var $nonHtmlCase = $mainPopup.find('[data-non-html-case]');

        if (!html) {
            $htmlCase.remove();
        } else {
            $nonHtmlCase.remove();

            $htmlCase.find('textarea').on('click', function () {
                $(this).select();
            });
        }

        $mainPopup.find('[data-custom-modal-text]').text(value);
        $mainPopup.fadeIn(250);

        $mainPopup.off('click');
        $mainPopup.on('click', '[data-popup-close]', function (event) {
            event.preventDefault();

            $('[data-popup="main"').fadeOut(250);
        });
    }

    function getScoreBoxBackgroundColor(score) {
        var color;

        if (score === '-') {
            color = '#ccc';
        } else if (score <= 3) {
            color = '#1f7def';
        } else if (score <= 6) {
            color = '#0cb737';
        } else if (score <= 8) {
            color = '#ee7600';
        } else {
            color = '#ed2c1e';
        }

        return color;
    }

    return {
        setScoreBoxesBackgroundColor: setScoreBoxesBackgroundColor,
        openModalPopup: openModalPopup
    }
};

var ReviewsHelper = function () {
    var votingInProgress = false;
    var exportInProgress = false;

    function setReviewVotingVariables() {
        $('.review').each(function () {
            var $this = $(this);
            var $thumbs = $this.find('.icon-upvote');
            var $upvotesDisplayBox = $this.find('.review-upvotes h4 i');
            var noUpvotes = ($upvotesDisplayBox.text() === '+ 0');
            var alreadyUpvoted = $thumbs.data('already-upvoted');

            if (noUpvotes) {
                $upvotesDisplayBox.hide();
            }

            if ($upvotesDisplayBox.text())

                if (alreadyUpvoted === 'True') {
                    $thumbs.removeClass('glyphicon-thumbs-up').addClass('glyphicon-thumbs-down');
                }
        });
    }

    function upvoteReview($this) {
        if (votingInProgress) {
            return;
        }

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
                changeDisplayValue();
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

        function changeDisplayValue() {
            var $displayBox = $this.closest('.review').find('.review-upvotes h4 i');
            var currentValue = $displayBox.text().substr(1);
            var newValue;

            if (isUpvote) {
                newValue = parseInt(currentValue) + 1;

                $displayBox.text('+ ' + newValue);

                $displayBox.show();
            } else {
                newValue = parseInt(currentValue) - 1;

                $displayBox.text('+ ' + newValue);

                if (newValue === 0) {
                    $displayBox.hide();
                }
            }
        }
    }

    function exportReviewToHtml($this) {
        if (exportInProgress) {
            return;
        }

        $.ajax({
            url: '/Reviews/ExportHtml/',
            type: 'GET',
            data: {
                id: $this.data('html-export-review')
            },
            beforeSend: function () {
                exportInProgress = true;
            },
            success: function (result) {
                genericHelper.openModalPopup(true, result);
            }, complete: function () {
                exportInProgress = false;
            }
        });
    }

    function truncateReviews() {
        var maxLength = 200;

        $('.review-content').each(function () {
            var $this = $(this);
            var reviewText = $this.text();

            if (reviewText.length <= maxLength) {
                return;
            }

            $this.html(
                reviewText.slice(0, maxLength) + '<span> ...<span> <br /><a href="#" class="more"><b>Read more</b></a>' +
                '<span style="display:none;">' + reviewText.slice(maxLength, reviewText.length) + ' <br /><a href="#" class="less">Less</a></span>'
                );

            $('#beach-details-container').on('click', '.more', function (event) {
                event.preventDefault();

                $(this).hide().prev().hide();
                $(this).next().show();
            });

            $('#beach-details-container').on('click', '.less', function (event) {
                event.preventDefault();

                $(this).parent().hide().prev().show().prev().show();
            });
        });
    }

    return {
        setReviewVotingVariables: setReviewVotingVariables,
        upvoteReview: upvoteReview,
        exportReviewToHtml: exportReviewToHtml,
        truncateReviews: truncateReviews
    }
};

var BeachesHelper = function () {
    var exportInProgress = false;

    function exportBeachToHtml($this) {
        if (exportInProgress) {
            return;
        }

        $.ajax({
            url: '/Beaches/ExportHtml/',
            type: 'GET',
            data: {
                id: $this.data('html-export-beach')
            },
            beforeSend: function () {
                exportInProgress = true;
            },
            success: function (result) {
                genericHelper.openModalPopup(true, result);
            }, complete: function () {
                exportInProgress = false;
            }
        });
    }

    function hideEmptyAsideElements() {
        $('.beach-aside').find('.aside-element').each(function () {
            var $this = $(this);
            var captureGroups = /\w+:\s*(\w+)/.exec($this.text());
            var elementEmpty = (captureGroups === null);

            if (elementEmpty) {
                $this.hide();
            }
        });
    }

    return {
        hideEmptyAsideElements: hideEmptyAsideElements,
        exportBeachToHtml: exportBeachToHtml
    }
};

var genericHelper = new GenericHelper();
var reviewsHelper = new ReviewsHelper();
var beachesHelper = new BeachesHelper();