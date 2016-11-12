(function ($) {
    $('.dashboard-link').on('click', function () {
        var action = $(this).text();
        var url = (action === 'Settings') ? '/Manage/Index/' : ('/User/' + action + '/');

        $.ajax({
            url: url,
            type: 'GET',
            success: function (result) {
                var dataTablesManager = new DataTablesManager();

                dataTablesManager.initializeDataTable(result);
            },
            error: function (data) {
                console.log(data);
            }
        });
    });
})(jQuery);