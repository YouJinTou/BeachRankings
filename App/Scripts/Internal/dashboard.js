(function ($) {
    $('.dashboard-link').on('click', function () {
        var action = $(this).text();
        var url = (action === 'Settings') ? '/Manage/Index/' : ('/User/' + action + '/');

        $.ajax({
            url: url,
            type: 'GET',
            success: function (result) {
                var isStatisticsModule = (action.indexOf('Statistics') > -1);

                //if (isStatisticsModule) {
                    var dataTablesManager = new DataTablesManager();

                    dataTablesManager.initializeDataTable(result);
                //}                

                setInfiniteScrolling(url);
            },
            error: function (data) {
                console.log(data);
            }
        });
    });

    function setInfiniteScrolling(url) {
        var isImagesModule = (url.indexOf('Images') > -1);

        if (!isImagesModule) {
            return;
        }

        var infiniteScroller = new InfiniteScroller('user-images-container', 'images');

        infiniteScroller.setInifiniteScroll();
    }
})(jQuery);