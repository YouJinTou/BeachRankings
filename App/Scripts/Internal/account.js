(function ($) {
    var $blog = $('#blog');

    showBlogsTextBoxOnModelErrors();

    $('#checkbox-blogger').on('click', function () {
        $blog.toggle(200);
    });

    $('#btn-submit').on('click', function (event) {
        event.preventDefault();

        var noBlog = $blog.is(':visible') && $('#blog input').val().trim().length === 0;

        if (noBlog) {
            if (anyTextBoxEmpty()) { // Show validation errors
                $('#register-form').submit();
            }

            var valSummary = $('[data-valmsg-summary] ul');

            valSummary.find('li').remove('.blog-val');
            valSummary.append('<li class="blog-val">The blog field is required.</li>');
        } else {
            $('#register-form').submit();
        }
    });

    $('.info-icon').on('mouseenter mouseleave', function () {
        $(this).parent().siblings('.custom-popup').toggle();
    });

    function showBlogsTextBoxOnModelErrors() {
        if ($('#checkbox-blogger').is(':checked')) {
            $blog.show();
        }
    }

    function anyTextBoxEmpty() {
        var anyEmpty = false;

        $('#register-container input[type=text]').each(function () {
            var isEmpty = ($(this).val().trim().length === 0);

            if (isEmpty) {
                anyEmpty = true;
            }
        });

        return anyEmpty;
    }
})(jQuery);