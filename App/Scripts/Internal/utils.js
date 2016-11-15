﻿var Helper = function () {
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

    function toggleReviewThumbs() {
        $('.concise-review').each(function () {
            var $this = $(this);
            var alreadyUpvoted = $this.find('.icon-upvote').data('already-upvoted');

            if (alreadyUpvoted === 'true') {
                $this.removeClass('glyphicon-thumbs-up').addClass('glyphicon-thumbs-down');
            }
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
        } else  {
            color = '#ed2c1e';
        }

        return color;
    }

    return {
        setScoreBoxesBackgroundColor: setScoreBoxesBackgroundColor,
        toggleReviewThumbs: toggleReviewThumbs
    }
};

var helper = new Helper();