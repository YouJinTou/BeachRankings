(function ($) {
    setDashboardEvents();
    setSettingsEvents();
    setChangePasswordEvents();

    function setDashboardEvents() {
        $('.dashboard-link').on('click', function () {
            var action = $(this).text();
            var url = (action === 'Settings') ? '/Manage/Index/' : ('/User/' + action + '/');

            $.ajax({
                url: url,
                type: 'GET',
                success: function (result) {
                    var isStatisticsModule = (action.indexOf('Statistics') > -1);

                    $('#results-container').html(result);

                    if (isStatisticsModule) {
                        var dataTablesManager = new DataTablesManager();

                        dataTablesManager.initializeDataTable();
                    }                

                    setInfiniteScrolling(url);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        });
    }

    function setSettingsEvents() {
        $('.settings-container').on('click', 'a', function () {
            var $this = $(this);
            var actionType = $this.data('index');
            var url = (actionType === 'avatar') ? '/Manage/ChangeAvatar/' : '/Manage/ChangePassword/';

            $.ajax({
                url: url,
                type: 'GET',
                success: function (result) {
                    $('#results-container').html(result);
                }
            });
        });
    }

    function setChangePasswordEvents() {
        $('.settings-container').on('click', '[data-settings-back]', function () {
            $.ajax({
                url: '/Manage/Index/',
                type: 'GET',
                success: function (result) {
                    $('#results-container').html(result);
                }
            });
        });
    }

    function setInfiniteScrolling(url) {
        var isImagesModule = (url.indexOf('Images') > -1);

        if (isImagesModule) {
            var infiniteScroller = new InfiniteScroller('user-images-container', 'images');

            infiniteScroller.setInifiniteScroll();
        }
    }
})(jQuery);