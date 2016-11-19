(function () {
    var dragdealersManager = new DragdealersManager();

    dragdealersManager.initializeDragdealers();
    helper.setScoreBoxesBackgroundColor();

    tryLoadExistingArticleLinks();

    $('.add-more').on('click', function () {
        var $linksContainer = $('#links-container');
        var lastLinkFieldEmpty = ($linksContainer.find('input').last().val().length === 0);

        if (lastLinkFieldEmpty) {
            return;
        }

        $linksContainer.children().first().clone().val('').appendTo($linksContainer);
    });

    $('[data-btn-submit-review]').on('click', function (event) {
        event.preventDefault();

        var articleLinkSeparator = '@';
        var blogLinks = $.map($('.blog-link'), function (link) {
            return $(link).val() + articleLinkSeparator;
        });

        $('#hdn-article-links').val(blogLinks)
        $('#hdn-sand-quality').val($('[data-sand-quality-handle]').text());
        $('#hdn-beach-cleanliness').val($('[data-beach-cleanliness-handle]').text());
        $('#hdn-beautiful-scenery').val($('[data-beautiful-scenery-handle]').text());
        $('#hdn-crowd-free').val($('[data-crowd-free-handle]').text());
        $('#hdn-water-purity').val($('[data-water-purity-handle]').text());
        $('#hdn-wastefree-seadbed').val($('[data-wastefree-seabed-handle]').text());
        $('#hdn-feet-friendly-bottom').val($('[data-feet-friendly-bottom-handle]').text());
        $('#hdn-sea-life-diversity').val($('[data-sea-life-diversity-handle]').text());
        $('#hdn-coral-reef').val($('[data-coral-reef-handle]').text());
        $('#hdn-walking').val($('[data-walking-handle]').text());
        $('#hdn-snorkeling').val($('[data-snorkeling-handle]').text());
        $('#hdn-kayaking').val($('[data-kayaking-handle]').text());
        $('#hdn-camping').val($('[data-camping-handle]').text());
        $('#hdn-infrastructure').val($('[data-infrastructure-handle]').text());
        $('#hdn-long-term-stay').val($('[data-long-term-stay-handle]').text());

        $('#submit-review-form').submit();
    });

    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });

    $('.info-icon').on('mouseenter mouseleave', function () {
        $(this).parent().siblings('.custom-popup').toggle();
    });

    function tryLoadExistingArticleLinks() {
        if ($('#hdn-article-links').val().length === 0) {
            return;
        }

        $('#hdn-article-links').val().split('@').forEach(function (link) {
            $('#links-container').children()
                .first()
                .clone()
                .val(link)
                .appendTo($('#links-container'));
        });

        $('#links-container').children().filter(function () {
            return ($(this).val().length === 0);
        }).remove();
    }
})();