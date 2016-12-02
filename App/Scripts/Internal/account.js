(function ($) {
    var $blog = $('#register-blog');

    showBlogsTextBoxOnModelErrors();

    $('#register-container').on('click', '#checkbox-blogger', function () {
        $blog.toggle(200);
    });

    $('#register-container').on('click', '#btn-submit', function (event) {
        event.preventDefault();

        var noBlog = $blog.is(':visible') && $('#register-blog input').val().trim().length === 0;

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