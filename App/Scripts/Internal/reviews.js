(function () {
    var dragdealersManager = new DragdealersManager();

    dragdealersManager.initializeDragdealers();
    helper.setScoreBoxesBackgroundColor();

    $('[data-btn-submit-review]').on('click', function (event) {
        event.preventDefault();

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
        $(this).parent().siblings('.review-popup').toggle();
    });
})();