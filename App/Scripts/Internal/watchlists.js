(function ($) {
    $('#results-container').on('click', '[data-html-export-watchlist]', function () {
        genericHelper.exportToHtml('Watchlists', 'ExportHtml', $(this).data('html-export-watchlist'));
    });
})(jQuery);