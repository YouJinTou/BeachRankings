(function ($) {
    setDashboardEvents();
    setSettingsEvents();

    function setDashboardEvents() {
        $('.dashboard-link').on('click', function () {
            var action = $(this).text();
            var url = (action.indexOf('Settings') > -1) ? '/Manage/Index' : ('/User/' + action + '/');

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
        $('.settings-container').on('click', '[data-index]', function () {
            var $this = $(this);
            var actionType = $this.data('index');
            var url = (actionType === 'avatar') ? '/User/ChangeAvatar/' : '/Manage/ChangePassword/';
            var changingAvatar = (actionType === 'avatar');

            $.ajax({
                url: url,
                type: 'GET',
                success: function (result) {
                    $('#results-container').html(result);

                    if (changingAvatar) {
                        setAvatarOnSubmitEvents();
                    }
                }
            });

            function setAvatarOnSubmitEvents() {
                var form = $('#avatar-form');

                $("#delete-avatar-link").on('click', function () {
                    form.attr('action', '/User/DeleteAvatar');
                    form.submit();
                });

                $("#save-avatar-btn").on('click', function () {
                    form.attr('action', '/User/ChangeAvatar');
                    form.submit();
                });
            }
        });

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