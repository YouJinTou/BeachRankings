(function ($) {
    setAutocompleteEvents();

    function setAutocompleteEvents() {
        var $mainSearchField = $('#main-search-field');
        var $autocompleteBox = $('.home-autocomplete-box');
        var insideSearchArea = 0; // bool
        var resultsCache = {};
        
        $mainSearchField.on('keyup', function (event) {            
            if (isValidKeyAction(event)) {
                return;
            }

            if (isEmpty()) {
                return;
            }

            var prefix = $mainSearchField.val();

            if (prefix.length === 0) {
                $autocompleteBox.hide();

                return;
            }

            if (keyCached(prefix)) {
                $autocompleteBox.html(resultsCache[prefix]);
                $autocompleteBox.show();

                return;
            }

            if (isDelete(event) && keyCached(prefix)) {
                $autocompleteBox.html(resultsCache[prefix]);
                $autocompleteBox.show();

                return;
            }

            $.ajax({
                url: '/Home/Autocomplete/',
                type: 'GET',
                data: {
                    prefix: prefix
                },
                success: function (result) {
                    var noResults = ((result.indexOf('li') <= 0) && (prefix.length >= 1));

                    if (noResults) {
                        result = '<ul><li><i>No results found</i></li></ul>';
                    }

                    $autocompleteBox.html(result);
                    resultsCache[prefix] = result;

                    $autocompleteBox.show();
                },
                error: function (data) {
                    result = '<ul><li><i>Something went wrong, for which we apologize</i></li></ul>';

                    $autocompleteBox.html(result);
                    $autocompleteBox.show();
                }
            });
        });

        $mainSearchField.on('click', function () {
            if ($mainSearchField.val().length <= 0) {
                return;
            }

            var items = $autocompleteBox.find('li');
            insideSearchArea = 1;

            items.removeClass('selected-item');
            $autocompleteBox.show();
        });

        $('body').on('click', '.autocomplete-item a', function (event) {
            var url = $(this).attr('href');
            var isBeach = (url.indexOf('/Beaches/') > -1);

            if (isBeach) {
                return;
            }

            event.preventDefault();

            $autocompleteBox.hide();

            var url = $(this).attr('href');

            getAutocompleteResults(url);
        });

        $($mainSearchField).on('mouseenter mouseleave', function () {
            insideSearchArea ^= 1;
        });        

        $(document).on('click', function () {
            if (!insideSearchArea) {
                $autocompleteBox.hide();
            }
        });

        function isValidKeyAction(event) {
            var noResults = $($autocompleteBox.find('li')).first().text() === 'No results found';

            if (noResults) {
                return false;
            }

            var isKeyUp = (event.keyCode === 38);
            var isKeyDown = (event.keyCode === 40);
            var isEnter = (event.keyCode === 13);

            if (isKeyUp || isKeyDown) {
                traverseResults(isKeyUp);

                return true;
            }

            if (isEnter) {
                $autocompleteBox.hide();

                var selectedItem = $autocompleteBox.find('.selected-item a').first();
                var url = selectedItem.attr('href');
                var isBeach = (url.indexOf('/Beaches/') > -1);

                if (isBeach) {
                    window.location.href = url;

                    return;
                }

                getAutocompleteResults(url);

                return true;
            }

            return false;
        }

        function isEmpty() {
            var term = $mainSearchField.val();
            var lastCharIsEmpty = (term[term.length - 1] === ' ');

            return lastCharIsEmpty;
        }

        function keyCached(prefix){
            return (prefix in resultsCache);
        }

        function isDelete(event) {
            return (event.keyCode === 8 || event.keyCode === 46);
        }

        function traverseResults(isKeyUp) {
            var selectedClass = 'selected-item';
            var items = $autocompleteBox.find('li');
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
            var $loadingImage = $('#loading-main-results-image');

            $loadingImage.show();

            $.ajax({
                url: url,
                type: 'GET',
                success: function (result) {
                    var dataTablesManager = new DataTablesManager();

                    $('#results-container').html(result);
                    
                    dataTablesManager.initializeDataTable();                 
                },
                complete: function () {
                    $loadingImage.hide();
                },
                error: function (data) {
                    $loadingImage.hide();
                }
            });
        }       
    }
})(jQuery);