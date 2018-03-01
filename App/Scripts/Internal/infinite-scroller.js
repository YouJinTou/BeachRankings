var InfiniteScroller = function (containerId, type, pageSize) {
    var page = 1;
    var pageSize = (typeof pageSize === 'undefined') ? 10 : pageSize;
    var containerSelector = '#' + containerId;
    var $resultsContainer = $(containerSelector);
    var footer = $('#main-footer');
    var inProgress = false;
    var allResultsShown = false;
    var isBeach = (type === 'beach');
    var isLocation = (type === 'location');
    var isImages = (type === 'images');

    genericHelper.setScoreBoxesBackgroundColor();

    function setInfiniteScroll() {
        $(window).scroll(function () {
            var resultsEnd = (footer.offset().top);
            var viewEnd = ($(window).height() + $(window).scrollTop());
            var distanceToEnd = (resultsEnd - viewEnd);
            var shouldLoad = (distanceToEnd < 200);
            var parameters = getParameters(type);
            var userNavigatedAway =
                parameters.url === '/Beaches/Reviews' &&
                parameters.data.beachId === undefined;

            if (!shouldLoad || inProgress || allResultsShown || userNavigatedAway) {
                return;
            }

            $.ajax({
                url: parameters.url,
                type: 'GET',
                data: parameters.data,
                beforeSend: function () {
                    siteManager.showSpinner();
                    inProgress = true;
                },
                success: function (result) {
                    if (!result) {
                        allResultsShown = true;

                        return;
                    }

                    var appendee = getAppendee(result);

                    $resultsContainer.append(appendee);

                    setTypeSpecificSettings();

                    page++;
                },
                error: function () {
                    siteManager.hideSpinner();
                },
                complete: function () {
                    siteManager.hideSpinner();
                    inProgress = false;
                }
            });
        });
    }

    function getParameters(type) {
        var parameters = {};

        switch (type) {
            case 'beach': {
                parameters.url = '/Beaches/Reviews';
                parameters.data = {
                    beachId: $('[data-beach-id]').data('beach-id'),
                    page: page,
                    pageSize: pageSize
                }

                return parameters;
            }
            case 'location': {
                var captureGroups = /.+(\/Countries|\/\w+Divisions|\/WaterBodies|\/Continents|\/World)(\/\w+)\/?(\d+)?/i.exec(window.location.href);
                parameters.url = (captureGroups[1] + captureGroups[2]);
                parameters.data = {
                    id: captureGroups[3],
                    page: page,
                    pageSize: pageSize
                };

                return parameters;
            }
            case 'images': {
                parameters.url = '/User/Images';
                parameters.data = {
                    page: page,
                    pageSize: pageSize
                };

                return parameters;
            }
            default:
                return undefined;
        }
    }

    function getAppendee(originalResult) {
        var $dummyDiv = $('<div>');
        var $appendee = $dummyDiv.append(originalResult).find(containerSelector);

        return ($appendee.length > 0) ? $appendee.html() : originalResult;
    }

    function setTypeSpecificSettings() {
        if (isBeach) {
            genericHelper.setScoreBoxesBackgroundColor();
            reviewsHelper.setReviewVotingVariables();
            reviewsHelper.truncateReviews();
        } else if (isLocation) {
            genericHelper.setScoreBoxesBackgroundColor();
        }
    }

    return {
        setInifiniteScroll: setInfiniteScroll
    }
};