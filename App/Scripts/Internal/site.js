(function ($) {
    $('#results-container').on('mouseenter mouseleave', '.info-icon', function () {
        $(this).parent().siblings('.custom-popup').toggle();
    });
})(jQuery);