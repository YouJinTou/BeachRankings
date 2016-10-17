(function ($) {
    var searchFieldBox = '#main-search-field';
    var autocompleteBox = '#search-autocomplete-box';
    var $mainSearchField = $(searchFieldBox);

    $mainSearchField.on('keyup', function () {
        $.ajax({
            url: '/Home/Autocomplete/',
            type: 'GET',
            data: {
                prefix: $mainSearchField.val()
            },
            success: function (result) {                
                $(autocompleteBox).html(result);
            },
            error: function (data) {
                console.log(data);
            }
        });
    });
})(jQuery);