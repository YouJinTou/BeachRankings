(function ($) {
    initializeSearch();
    setAutocompleteEvents();

    function initializeSearch() {
        $.ajax({
            url: '/Home/Autocomplete/',
            type: 'GET',
            data: {
                prefix: 'bu'
            }
        });
    }

    function setAutocompleteEvents() {
        var $mainSearchField = $('#main-search-field');
        var $autocompleteBox = $('.home-autocomplete-box');
        var autocompleteSelected = 0;

        $mainSearchField.on('keyup', function (event) {
            if (isValidKeyAction(event)) {
                return;
            }

            $.ajax({
                url: '/Home/Autocomplete/',
                type: 'GET',
                data: {
                    prefix: $mainSearchField.val()
                },
                success: function (result) {
                    var inputLength = $mainSearchField.val().length;
                    var noResults = ((result.indexOf('li') <= 0) && (inputLength > 1));

                    $autocompleteBox.show();

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

        $mainSearchField.on('click', function () {
            if ($mainSearchField.val().length <= 1) {
                return;
            }

            $mainSearchField.keyup();
        });

        $('body').on('click', '.autocomplete-item a', function (event) {
            event.preventDefault();

            $autocompleteBox.hide();

            var url = $(this).attr('href');

            getAutocompleteResults(url);
        });

        $('.home-search-header').on('hover', function () {
            autocompleteSelected ^= 1;
        });        

        $(document).on('click', function () {
            if (!autocompleteSelected) {
                $autocompleteBox.hide();
            }
        });

        function isValidKeyAction(event) {
            var isKeyUp = (event.keyCode === 38);
            var isKeyDown = (event.keyCode === 40);
            var isEnter = (event.keyCode === 13);

            if (isKeyUp || isKeyDown) {
                traverseResults(isKeyUp);

                return true;
            }

            if (isEnter) {
                $autocompleteBox.hide();

                var url = $autocompleteBox.find('.selected-item a').first().attr('href');

                getAutocompleteResults(url);

                return true;
            }

            return false;
        }

        function traverseResults(isKeyUp) {
            var selectedClass = 'selected-item';
            var items = $autocompleteBox.find('li');
            var noResults = $(items[0]).text() === 'No results found';

            if (noResults) {
                return;
            }

            var selected = $autocompleteBox.find('.selected-item').first();
            var selectedIndex = $autocompleteBox.find('.selected-item').prevAll().length;

            if (!selected.length) {
                items.first().addClass(selectedClass);

                return;
            }

            selected.removeClass(selectedClass);

            if (isKeyUp) {
                if (selectedIndex === 0) {
                    items.last().addClass(selectedClass);
                }

                selected.prev('li').addClass(selectedClass);

                return;
            }

            var lastIndexReached = (items.length - 1 === selectedIndex);

            if (lastIndexReached) {
                items.first().addClass(selectedClass);
            }

            selected.next('li').addClass(selectedClass);
        }

        function getAutocompleteResults(url) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (result) {
                    $('#beaches-search-result').html(result);
                    $('body').height('#beaches-search-result');
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }       
    }    
})(jQuery);