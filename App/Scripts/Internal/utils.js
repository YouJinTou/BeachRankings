var GenericHelper = function () {
    var exportInProgress = false;

    function getViewportWidth() {
        var viewportHeight = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);

        return viewportHeight;
    }

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

    function openModalPopup(isHtml, value) {
        var $mainPopup = $('[data-popup="main"');
        var $exportCase = $mainPopup.find('[data-export-case]');
        var $nonExportCase = $mainPopup.find('[data-non-export-case]');

        if (!isHtml) {
            $exportCase.remove();

            $mainPopup.find('[data-custom-modal-text]').text(value);
        } else {
            var horizontalHtml = value.split('@@@')[0];
            var verticalHtml = value.split('@@@')[1];

            if (horizontalHtml) {
                renderHtmlAsImage('[data-html-renderable]', horizontalHtml,
                    '#exportable-table-horizontal', '#export-result-horizontal', true);
            }

            if (verticalHtml) {
                renderHtmlAsImage('[data-html-renderable]', verticalHtml,
                    '#exportable-table-vertical', '#export-result-vertical', true);
            }

            $nonExportCase.remove();

            $exportCase.find('textarea').on('click', function () {
                $(this).select();
            });
        }

        $mainPopup.fadeIn(250);

        $mainPopup.off('click');
        $mainPopup.on('click', '[data-popup-close]', function (event) {
            event.preventDefault();

            $('[data-popup="main"').fadeOut(250);
        });
    }

    function exportToHtml(controller, action, id) {
        if (exportInProgress) {
            return;
        }

        $.ajax({
            url: '/' + controller + '/' + action + '/',
            type: 'GET',
            data: {
                id: id
            },
            beforeSend: function () {
                siteManager.showSpinner();
                exportInProgress = true;
            },
            success: function (result) {
                openModalPopup(true, result);
            }, complete: function () {
                siteManager.hideSpinner();
                exportInProgress = false;
            }, error: function () {
                siteManager.hideSpinner();
            }
        });
    }

    function renderHtmlAsImage(
        tempStorageSelector,
        htmlString,
        rootElementSelector,
        whereRenderSelector,
        shouldEmptyWhereRenderContainer) {
        $(tempStorageSelector).append(htmlString);

        html2canvas(document.querySelector(rootElementSelector)).then(function (canvas) {
            if (shouldEmptyWhereRenderContainer) {
                $(whereRenderSelector).empty();
            }

            $(whereRenderSelector).append(canvas);
            $(whereRenderSelector).find('canvas').width('100%').height('100%');

            $(tempStorageSelector).empty();
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
        getViewportWidth: getViewportWidth,
        setScoreBoxesBackgroundColor: setScoreBoxesBackgroundColor,
        openModalPopup: openModalPopup,
        exportToHtml: exportToHtml,
        renderHtmlAsImage: renderHtmlAsImage
    }
};
var ReviewsHelper = function () {
    var votingInProgress = false;

    function setReviewVotingVariables() {
        $('.review').each(function () {
            var $this = $(this);
            var $thumbs = $this.find('.icon-upvote');
            var $upvotesDisplayBox = $this.find('.upvotes-info');
            var noUpvotes = ($upvotesDisplayBox.text() === '0 upvotes');
            var alreadyUpvoted = $thumbs.data('already-upvoted');

            if (noUpvotes) {
                $this.find('.no-upvotes-info').show();
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
            var $displayBox = $this.closest('.review').find('.upvotes-info');
            var $noUpvotesBox = $this.closest('.review').find('.no-upvotes-info');
            var currentValue = $displayBox.text().substr(0, 1);
            console.log(currentValue)
            var newValue;

            if (isUpvote) {
                newValue = parseInt(currentValue) + 1;

                $displayBox.text(newValue + ((newValue === 1) ? ' upvote' : ' upvotes'));

                $displayBox.show();
                $noUpvotesBox.hide();
            } else {
                newValue = parseInt(currentValue) - 1;

                $displayBox.text(newValue + ((newValue === 1) ? ' upvote' : ' upvotes'));

                if (newValue === 0) {
                    $displayBox.hide();
                    $noUpvotesBox.show();
                }
            }
        }
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
                reviewText.slice(0, maxLength) + '<div class="disable-block-inline">... </div><a href="#" class="more"><b>More</b></a>' +
                '<span style="display:none;">' + reviewText.slice(maxLength, reviewText.length) + '<a href="#" class="less">Less</a></span>'
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
        truncateReviews: truncateReviews
    }
};
var BeachesHelper = function () {
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

    function setElementsResponsive() {
        var viewportWidth = genericHelper.getViewportWidth();

        if (viewportWidth <= 768) {
            var aside = $('.beach-aside-box').insertAfter('.beach-head');
        }
    }

    return {
        hideEmptyAsideElements: hideEmptyAsideElements,
        setElementsResponsive: setElementsResponsive,
    }
};
var BlogHelper = function () {
    function tryLoadExistingArticleLinks() {
        if ($('#hdn-article-links').val().length === 0) {
            return;
        }

        $('#hdn-article-links').val().split('@').forEach(function (link) {
            $('#links-container').children()
                .first()
                .clone()
                .val(link)
                .appendTo($('#links-container'));
        });

        $('#links-container').children().filter(function () {
            return ($(this).val().length === 0);
        }).remove();
    }

    function setBlogArticleLinks() {
        var articleLinkSeparator = '@';
        var blogLinks = $.map($('.blog-link'), function (link) {
            return $(link).val() + articleLinkSeparator;
        });

        $('#hdn-article-links').val(blogLinks)
    }

    return {
        tryLoadExistingArticleLinks: tryLoadExistingArticleLinks,
        setBlogArticleLinks: setBlogArticleLinks
    }
};
var WatchlistsHelper = function () {
    var isInsideWatchlistsPartial = 0; // bool
    var $currentWatchlist = $();
    var watchlistFetchInProgress = false;

    function registerWatchlistEvents() {
        $('#results-container').on('mouseenter mouseleave', '.watchlist-overview-container', function () {
            isInsideWatchlistsPartial ^= 1;
        });

        $(document).on('click', function () {
            if (!isInsideWatchlistsPartial) {
                $('.watchlist-overview-container').hide();
            }
        });

        $('#results-container').on('click', '.watchlist-trigger', function () {
            if ($currentWatchlist.is(':visible')) {
                return;
            }

            var $this = $(this);

            $.ajax({
                url: '/Watchlists/UserWatchlistsByBeachId/',
                type: 'GET',
                data: {
                    beachId: $this.data('watchlist-trigger')
                },
                beforeSend: function () {
                    watchlistFetchInProgress = true;
                },
                success: function (result) {
                    $this.parent().find('.watchlist-overview').html(result);

                    $currentWatchlist = $this.next().find('.watchlist-overview-container');

                    replaceWithMovableIfInTable($this);
                },
                complete: function () {
                    watchlistFetchInProgress = false;
                }
            });
        });

        $('#results-container').on('click', '.cb-adjustable-watchlist', function () {
            $(this).closest('form').submit();
        });

        $('#results-container').on('click', '.new-watchlist-btn', function () {
            $(this).closest('.new-watchlist-box').find('.new-watchlist-form').show();
        });

        $('#results-container').on('click', '.submit-new-watchlist', function () {
            var $this = $(this);
            var name = $this.closest('.new-watchlist-form').find('.new-watchlist-name').val();

            if (name.length === 0) {
                return;
            }

            var beachId = $this
                .closest('.watchlist-overview-container')
                .find('[data-beach-id]')
                .data('beach-id');
            var viewModel = {
                beachId: beachId,
                name: name
            };

            $.ajax({
                url: '/Watchlists/Add/',
                type: 'Post',
                data: {
                    viewModel: viewModel
                },
                success: function (result) {
                    var isFullView = !beachId;

                    if (isFullView) {
                        refreshFullView();
                    } else {
                        attachNewListToConciseView($this, result, beachId);
                    }
                }
            });
        });

        $('#results-container').on('click', '[data-remove-watchlist]', function () {
            var toDelete = confirm('Are you sure you want to delete this watchlist?');

            if (!toDelete) {
                return;
            }

            var $this = $(this);

            $.ajax({
                url: '/Watchlists/Delete',
                data: {
                    id: $(this).data('remove-watchlist')
                },
                type: 'POST',
                success: function () {
                    var $watchlistRow = $this.closest('.watchlist-row');
                    var $hr = $watchlistRow.next('hr');
                    var $watchlistsWrapper = $this.closest('.watchlists-wrapper');
                    var watchlistsLeft = $watchlistsWrapper.find('.watchlist-row').length - 1;

                    $watchlistRow.remove();
                    $hr.remove();

                    if (watchlistsLeft === 0) {
                        $watchlistsWrapper.find('hr').remove();
                    }
                },
                error: function (data) {
                    console.log(data);
                }
            });
        });

        $('#results-container').on('click', '[data-edit-watchlist]', function () {
            $(this).closest('.watchlist-row').find('.watchlist-edit-controls').show();
        });

        $('#results-container').on('click', '[data-btn-edit-watchlist]', function () {
            var $this = $(this);
            var $editControls = $this.closest('.watchlist-edit-controls');
            var name = $editControls.find('[data-watchlist-edit-name]').val();

            if (!name) {
                return;
            }

            var viewModel = {
                id: $this.data('btn-edit-watchlist'),
                name: name
            };

            $.ajax({
                url: '/Watchlists/Edit',
                data: {
                    viewModel: viewModel
                },
                type: 'POST',
                success: function () {
                    $this.closest('.watchlist-row').find('[data-watchlist-name]').text(viewModel.name);
                    $editControls.hide();
                },
                error: function (data) {
                    console.log(data);
                }
            });
        });

        $('#results-container').on('click', '[data-cancel-watchlist]', function () {
            $(this).closest('.watchlist-cancel').hide();
        });

        function refreshFullView() {
            location.reload();
        }

        function attachNewListToConciseView($this, result, beachId) {
            $this
                .closest('.watchlist-overview-container')
                .find('.watchlist-list-item-container')
                .append(result);
            var $allListItems = $this
                .closest('.watchlist-overview-container')
                .find('.concise-watchlist');
            var totalCount = $allListItems.length;
            var $appended = $allListItems.last();

            $appended.find('input').each(function () {
                var currentNameValue = $(this).attr('name');
                var newNameValue = 'Watchlists[' + (totalCount - 1) + '].' + currentNameValue;

                $(this).removeAttr('name');
                $(this).attr('name', newNameValue);
            });

            $appended.find('[data-beach-id]').val(beachId);
        }

        function replaceWithMovableIfInTable($trigger) {
            if ($currentWatchlist.parents('#table-result_wrapper').length === 0) {
                return;
            }

            var $movableWatchlist = $('.movable-watchlist');

            $movableWatchlist.empty();
            $movableWatchlist.append($currentWatchlist);

            var $triggerPosition = $trigger.position();
            var watchlistHeight = $currentWatchlist.height();
            var triggerWidth = $trigger.outerWidth();
            var triggerHeight = $trigger.outerHeight();
            var watchlistTop = ($triggerPosition.top + (watchlistHeight) - triggerHeight);
            var watchlistLeft = ($triggerPosition.left + 3 * triggerWidth + 150);

            $movableWatchlist.css({
                position: 'absolute',
                top: watchlistTop + 'px',
                left: watchlistLeft + "px"
            });
        }
    }

    function onUpdateWatchlists() {
        setTimeout(hideTextContainer, 750);

        function hideTextContainer() {
            $('.concise-watchlist-update span').text('');
        }
    }

    return {
        registerWatchlistEvents: registerWatchlistEvents,
        onUpdateWatchlists: onUpdateWatchlists
    }
};

var genericHelper = new GenericHelper();
var reviewsHelper = new ReviewsHelper();
var beachesHelper = new BeachesHelper();
var blogHelper = new BlogHelper();
var watchlistsHelper = new WatchlistsHelper();