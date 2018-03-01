(function ($) {
    setDashboardMenuEvents();
    setSettingsEvents();
    setStatistics();
    setInfiniteScrolling();
    setImagesEvents();
    setWatchlistsEvents();
    setDataTablesEvents();

    function setDashboardMenuEvents() {
        var viewportWidth = genericHelper.getViewportWidth();

        $('#dashboard-dropdown-trigger').on('click', function () {
            $('#dashboard-dropdown').toggle(200);
        });

        if (viewportWidth >= 768) {
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

            $('#user-overview').on('mouseenter', function () {
                $('.user-overview-helper').show();
            });

            $('#user-overview').on('mouseleave', function () {
                $('.user-overview-helper').hide();
            });
        }
    }

    function setSettingsEvents() {
        setBlogEvents();

        $('#results-container').on('click', '[data-settings-index]', function () {
            var $this = $(this);
            var url = $this.data('settings-index');

            $.ajax({
                url: url,
                type: 'GET',
                success: function (result) {
                    $('#results-container').html(result);

                    if (url.indexOf('ChangeAvatar') > -1) {
                        setAvatarOnSubmitEvents();
                    }
                }
            });

            function setAvatarOnSubmitEvents() {
                var form = $('#avatar-form');

                $("#delete-avatar-link").off('click').on('click', function () {
                    var toDelete = confirm('Your avatar will be deleted and set to the default one. Proceed?');

                    if (toDelete) {
                        form.attr('action', '/User/DeleteAvatar');
                        form.submit();
                    }                   
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
        var isDataTablesModule =
            (window.location.href.indexOf('Statistics') > -1) ||
            (window.location.href.indexOf('Watchlist') > -1) ||
            (window.location.href.indexOf('Beaches/Best') > -1);

        if (isDataTablesModule) {
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

            if (!toDelete) {
                return;
            }

            var $this = $(this);

            $.ajax({
                url: '/User/DeleteBeachImage',
                data: {
                    id: $(this).data('remove-image')
                },
                type: 'POST',
                success: function () {
                    var $imageRow = $this.closest('.image-row');

                    $this.next('.dashboard-image-wrapper').remove();
                    $this.remove();

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
        });
    }

    function setWatchlistsEvents() {
        watchlistsHelper.registerWatchlistEvents();
    }

    function setDataTablesEvents() {
        $('#results-container').on('click', '.predefined-filter', function () {
            var $this = $(this);
            var $form = $this.closest('form');
            var controller = $form.find('[data-controller-name]').val();
            var action = 'FilteredStatistics';
            var placeId = $this.closest('form').find('[data-place-id]').val();
            var filterType = $this.data('predefined-filter-type');
            var url = '/' + controller + '/' + action + '?' + 'id=' + placeId + '&filterType=' + filterType;

            $form.attr('action', url);
            $form.submit();
        });
    }
})(jQuery);