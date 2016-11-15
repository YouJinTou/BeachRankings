var Helper = function () {
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

    function setVotingVariables() {
        $('.concise-review').each(function () {
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
        setVotingVariables: setVotingVariables
    }
};

var helper = new Helper();