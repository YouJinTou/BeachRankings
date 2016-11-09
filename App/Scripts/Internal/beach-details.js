(function ($) {
    helper.setScoreBoxesBackgroundColor();

    $('.slick-carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        arrows: false,
        mobileFirst: true
    });

    $('#delete-beach-a').on('click', function () {
        $('#delete-beach-form').submit();
    });    
})(jQuery);