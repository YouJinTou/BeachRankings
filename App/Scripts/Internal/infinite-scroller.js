var InfiniteScroller = function (containerId, type) {
    var page = 1;
    var containerSelector = '#' + containerId;
    var $resultsContainer = $(containerSelector);
    var footer = $('#main-footer');
    var inProgress = false;
    var allResultsShown = false;

    helper.setScoreBoxesBackgroundColor();

    function setInfiniteScroll() {
        $(window).scroll(function () {
            var resultsEnd = (footer.offset().top);
            var viewEnd = ($(window).height() + $(window).scrollTop());
            var distanceToEnd = (resultsEnd - viewEnd);
            var shouldLoad = (distanceToEnd < 200);

            if (!shouldLoad || inProgress || allResultsShown) {
                return;
            }

            var parameters = getParameters(type);

            $.ajax({
                url: parameters.url,
                type: 'GET',
                data: parameters.data,
                beforeSend: function () {
                    inProgress = true;
                },
                success: function (result) {
                    if (!result) {
                        allResultsShown = true;

                        return;
                    }

                    var appendee = getAppendee(result);

                    $resultsContainer.append(appendee);

                    helper.setScoreBoxesBackgroundColor($resultsContainer);

                    page++;
                },
                complete: function () {

                    inProgress = false;
                }
            });
        });
    }

    function getParameters(type) {
        var parameters = {};

        switch (type) {
            case 'location': {
                var captureGroups = /.+(\/Countries|\/\w+Divisions|\/WaterBodies)(.+\/)(\d+)/i.exec(window.location.href);
                parameters.url = (captureGroups[1] + captureGroups[2]);
                parameters.data = {
                    id: captureGroups[3],
                    page: page,
                    pageSize: 10
                };

                return parameters;
            }
            case 'images': {
                parameters.url = '/User/Images/';
                parameters.data = {
                    page: page,
                    pageSize: 10
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

    return {
        setInifiniteScroll: setInfiniteScroll
    }
};