(function ($) {
    var page = 0;

    $(window).scroll(function () {
        var viewportHeight = ($(document).height() - $(window).height());

        if ($(window).scrollTop() === viewportHeight) {
            $.ajax({
                url: '/Countries/Beaches/',
                type: 'GET',
                data: {
                    id: 2,
                    page: page,
                    pageSize: 5
                },
                success: function (result) {
                    $('#beaches-result').append(result);

                    page++;
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }
    });
})(jQuery);