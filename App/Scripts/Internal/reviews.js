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

    $('.info-icon').on('mouseenter mouseleave', function () {
        $(this).parent().siblings('.review-popup').toggle();
    });
})();