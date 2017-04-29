(function ($) {
    $('.add-more-links').on('click', function () {
        var $linksContainer = $('#links-container');
        var lastLinkFieldEmpty = ($linksContainer.find('input').last().val().length === 0);

        if (lastLinkFieldEmpty) {
            return;
        }

        $linksContainer.children().first().clone().val('').appendTo($linksContainer);
    });
})(jQuery);