(function ($) {
    if (document.getElementById('table-result')) {
        dataTablesManager.initializeDataTable();
    } else if (document.getElementById('contributors-table')) {
        dataTablesManager.initializeContributorsDataTable();
    }
})(jQuery);