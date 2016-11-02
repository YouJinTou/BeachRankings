(function ($) {
    var $mainSearchField = $('#main-search-field');
    var $autocompleteBox = $('.home-autocomplete-box');
    var autocompleteSelected = 0;

    $mainSearchField.on('keyup', function () {
        $.ajax({
            url: '/Home/Autocomplete/',
            type: 'GET',
            data: {
                prefix: $mainSearchField.val()
            },
            success: function (result) {
                var inputLength = $mainSearchField.val().length;
                var noResults = ((result.indexOf('li') <= 0) && (inputLength > 1));
                autocompleteSelected ^= 1;

                if (!autocompleteSelected) {
                    $autocompleteBox.show();
                }

                if (noResults) {
                    result = '<ul><li><i>No results found</i></li></ul>';
                }                

                $autocompleteBox.html(result);
            },
            error: function (data) {
                console.log(data);
            }
        });
    });

    $('.home-search-header').hover(function () {
        autocompleteSelected ^= 1;
    })

    $(document).on('click', function () {
        if (!autocompleteSelected) {
            $autocompleteBox.hide();
        }
    })
})(jQuery);