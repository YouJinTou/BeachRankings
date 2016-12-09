(function ($) {
    setDashboardMenuEvents();
    setSettingsEvents();
    setStatistics();
    setInfiniteScrolling();
    setImagesEvents();

    function setDashboardMenuEvents() {
        var menuVisible = false;

        $('#dashboard-dropdown-trigger').on('mouseenter', function () {
            $('#dashboard-dropdown').show();
        });

        $('#dashboard-dropdown-trigger').on('mouseleave', function () {
            setTimeout(hideMenu, 300);

            function hideMenu() {
                if (!menuVisible) {
                    $('#dashboard-dropdown').hide();
                }
            }
        });

        $('#dashboard-dropdown').on('mouseenter', function () {
            $(this).show();

            menuVisible = true;
        });

        $('#dashboard-dropdown').on('mouseleave', function () {
            $(this).hide();

            menuVisible = false;
        });

        $('#user-overview').on('mouseenter mouseleave', function () {
            $('.user-overview-helper').toggle(200);
        });
    }

    function setSettingsEvents() {
        setBlogEvents();

        $('#results-container').on('click', '[data-settings-index]', function () {
            var $this = $(this);
            var actionType = $this.data('settings-index');
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

                $("#delete-avatar-link").off('click').on('click', function () {
                    form.attr('action', '/User/DeleteAvatar');
                    form.submit();
                });

                $("#save-avatar-btn").off('click').on('click', function () {
                    form.attr('action', '/User/ChangeAvatar');
                    form.submit();
                });
            }
        });

        $('#results-container').on('click', '[data-settings-back]', function () {
            $.ajax({
                url: '/Manage/Index/',
                type: 'GET',
                success: function (result) {
                    $('#results-container').html(result);

                    setBlogEvents();
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

    function setStatistics() {
        var isStatisticsModule = (window.location.href.indexOf('Statistics') > -1);

        if (isStatisticsModule) {
            var dataTablesManager = new DataTablesManager();

            dataTablesManager.initializeDataTable();
        }
    }

    function setInfiniteScrolling() {
        var isImagesModule = (window.location.href.indexOf('Images') > -1);

        if (isImagesModule) {
            var infiniteScroller = new InfiniteScroller('user-images-container', 'images');

            infiniteScroller.setInifiniteScroll();
        }
    }

    function setImagesEvents() {
        $('#results-container').on('click', '[data-remove-image]', function () {
            var toDelete = confirm('Are you sure you want to delete this image?');

            if (toDelete) {
                var $this = $(this);

                $.ajax({
                    url: '/User/DeleteBeachImage',
                    data: {
                        id: $(this).data('remove-image')
                    },
                    type: 'POST',
                    success: function () {
                        var $imageRow = $this.closest('.image-row');

                        $this.closest('.dashboard-image-wrapper').remove();

                        var imagesLeft = $imageRow.find('.dashboard-image-wrapper').length;

                        if (imagesLeft === 0) {
                            $imageRow.next('hr').remove();
                            $imageRow.remove();
                        }
                    },
                    error: function (data) {
                        console.log(data);
                    }
                });
            }            
        });
    }
})(jQuery);