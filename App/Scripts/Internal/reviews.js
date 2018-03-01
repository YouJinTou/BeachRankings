(function () {
    var dragdealersManager = new DragdealersManager();

    dragdealersManager.initializeDragdealers();
    beachesHelper.setElementsResponsive();
    genericHelper.setScoreBoxesBackgroundColor();
    beachesHelper.hideEmptyAsideElements();
    blogHelper.tryLoadExistingArticleLinks();

    $('#btn-beach-export-html').on('click', function () {
        genericHelper.exportToHtml('Beaches', 'ExportHtml', $(this).data('html-export-beach'));
    });

    $('[data-btn-submit-review]').on('click', function (event) {
        event.preventDefault();
        blogHelper.setBlogArticleLinks();

        $('#hdn-sand-quality').val($('[data-sand-quality-handle]').text());
        $('#hdn-beach-cleanliness').val($('[data-beach-cleanliness-handle]').text());
        $('#hdn-beautiful-scenery').val($('[data-beautiful-scenery-handle]').text());
        $('#hdn-crowd-free').val($('[data-crowd-free-handle]').text());
        $('#hdn-infrastructure').val($('[data-infrastructure-handle]').text());
        $('#hdn-water-visibility').val($('[data-water-visibility-handle]').text());
        $('#hdn-litter-free').val($('[data-litter-free-handle]').text());
        $('#hdn-feet-friendly-bottom').val($('[data-feet-friendly-bottom-handle]').text());
        $('#hdn-sea-life-diversity').val($('[data-sea-life-diversity-handle]').text());
        $('#hdn-coral-reef').val($('[data-coral-reef-handle]').text());
        $('#hdn-snorkeling').val($('[data-snorkeling-handle]').text());
        $('#hdn-kayaking').val($('[data-kayaking-handle]').text());
        $('#hdn-walking').val($('[data-walking-handle]').text());
        $('#hdn-camping').val($('[data-camping-handle]').text());
        $('#hdn-long-term-stay').val($('[data-long-term-stay-handle]').text());
        $('#submit-review-form').submit();
    });

    $(".light-gallery").lightGallery(); 

    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });
})();