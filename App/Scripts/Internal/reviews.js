(function () {
    var dragdealersManager = new DragdealersManager();

    dragdealersManager.initializeDragdealers();
    helper.setScoreBoxesBackgroundColor();

    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });

    $('.drag-holder').on('mouseenter mouseleave', function () {
        $(this).find('.review-popup').toggle();
    });

    $('.info-icon').on('mouseenter mouseleave', function () {
        $(this).parent().siblings('.review-popup').toggle();
    });
})();