(function ($) {
    var page = 1;
    var resultsContainer = $('#beaches-result');
    var footer = $('#main-footer');
    var inProgress = false;
    var allResultsShown = false;

    $(window).scroll(function () {
        var resultsEnd = (footer.offset().top);
        var viewEnd = ($(window).height() + $(window).scrollTop());
        var distanceToEnd = (resultsEnd - viewEnd);
        var shouldLoad = (distanceToEnd < 200);

        if (!shouldLoad || inProgress || allResultsShown) {
            return;
        }

        var captureGroups = /.+(\/Countries|\/\w+Divisions|\/WaterBodies)(.+\/)(\d+)/i.exec(window.location.href);
        var url = (captureGroups[1] + captureGroups[2]);
        var $loadingImage = $('#loading-main-results-image');

        $loadingImage.show();

        $.ajax({
            url: url,
            type: 'GET',
            data: {
                id: captureGroups[3],
                page: page,
                pageSize: 10
            },
            beforeSend: function(){
                inProgress = true;
            },
            success: function (result) {
                if (!result) {
                    allResultsShown = true;

                    return;
                }

                resultsContainer.append(result);

                page++;
            },
            complete: function () {
                inProgress = false;

                $loadingImage.hide();
            },
            error: function (data) {
                $loadingImage.hide();
            }
        });
    });
})(jQuery);