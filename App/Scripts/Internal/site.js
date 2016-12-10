(function ($) {
    $('#results-container').on('click', '.info-icon', function () {
        $(this).siblings('.custom-popup').toggle();
    });
})(jQuery);