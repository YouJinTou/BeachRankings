(function ($) {
    var $blogs = $('#blogs');

    showBlogsTextBoxOnModelErrors();

    $('#checkbox-blogger').on('click', function () {
        $blogs.toggle(200);
    });

    $('#btn-submit').on('click', function (event) {
        event.preventDefault();

        var noBlogs = $('#blogs').is(':visible') && $('#blogs input').val().trim().length === 0;

        if (noBlogs) {
            if (anyTextBoxEmpty()) { // Show validation errors
                $('#register-form').submit();
            }

            var valSummary = $('[data-valmsg-summary] ul');

            valSummary.find('li').remove('.blogs-val');
            valSummary.append('<li class="blogs-val">The blogs field is required.</li>');
        } else {
            $('#register-form').submit();
        }
    });

    function showBlogsTextBoxOnModelErrors() {
        $('#register-container input[type=text]').each(function () {
            var isError = ($(this).val().trim().length > 0);

            if (isError) {
                $blogs.show();

                return false;
            }
        });
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