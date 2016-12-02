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
        setBlogEvents();

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

        function setBlogEvents() {
            var $blog = $('#settings-blog');
            var $btnSaveBlog = $('#btn-save-blog');
            var $checkBox = $('#settings-checkbox-blogger');
            var $blogUrl = $('#txt-settings-blog-url');
            var changed = false;

            if ($checkBox.is(':checked')) {
                $blog.show();
                $blogUrl.show();
            }

            $checkBox.on('click', function () {
                $blog.toggle(200);

                if (!changed) {
                    $btnSaveBlog.toggle();
                }
            });

            $blogUrl.on('keyup', function () {
                if (!$btnSaveBlog.is(':visible')) {
                    changed = true;

                    $btnSaveBlog.show();
                }
            });

            $('.settings-container').on('click', '#btn-save-blog', function (event) {
                event.preventDefault();

                var shouldSubmit = true;
                var noBlog = $blog.is(':visible') && $('#settings-blog input').val().trim().length === 0;
                var deletingBlog = !$checkBox.is(':checked');

                if (noBlog) {
                    var valSummary = $('[data-valmsg-summary] ul');
                    shouldSubmit = false;

                    valSummary.find('li').remove('.blog-val');
                    valSummary.append('<li class="blog-val">The blog field is required.</li>');
                } else if (deletingBlog) {
                    shouldSubmit = confirm("Are you sure you want to remove your blog and all the links you've posted?");
                }

                if (shouldSubmit) {
                    $('#manage-blog-form').submit();
                }
            });
        }        
    }

    function setInfiniteScrolling(url) {
        var isImagesModule = (url.indexOf('Images') > -1);

        if (isImagesModule) {
            var infiniteScroller = new InfiniteScroller('user-images-container', 'images');

            infiniteScroller.setInifiniteScroll();
        }
    }
})(jQuery);