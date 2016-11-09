(function ($) {
    var addIndex = window.location.href.indexOf('/Add');
    var mode = (addIndex > -1) ? 'add' : 'edit';
    var adding = (mode === 'add');
    var $ddlDivisions = $('[data-ddl-division]');
    var divisionHolder = '[data-division-holder]';
    var $textBoxName = $('[data-textbox-name]');
    var $beachNameContainer = $('[data-beach-name]');
    var $textBoxCoordinates = $('[data-textbox-coordinates]');
    var $validationSpan = $('[data-generic-validation-alert]');
    
    if (adding) {
        resetCountriesDropdown();
    } else {
        showFieldsInEditMode();
    }

    setBindings();
    setGeneralEvents();
    setEditEvents();

    function resetCountriesDropdown() {
        if ($($ddlDivisions[0]).val()) {
            $($ddlDivisions[0]).val('');
        }
    }

    function showFieldsInEditMode() {
        for (var i = 0; i < $ddlDivisions.length; i++) {
            var $ddl = $($ddlDivisions[i]);

            if ($ddl[0].length > 0) {
                $ddl.closest(divisionHolder).show();
            }
        }

        $beachNameContainer.show();
    }

    function setBindings() {
        $textBoxName.bind('paste', function (event) {
            event.preventDefault();
        });
    }    

    function setGeneralEvents() {
        $ddlDivisions.on('change', function () {
            var $this = $(this);            
            var nextDivision = $this.data('next-division');
            var url = $this.data('ddl-division') + $this.val();
            var currentAddress = getCurrentAddress();            

            $beachNameContainer.hide();

            if (!nextDivision) {
                setAutocomplete();

                $beachNameContainer.show();

                return;
            }

            var $nextDivision = $($ddlDivisions[nextDivision]);

            if (!$this.val()) {
                updateDivisionDropdowns(false);

                return;
            }

            $.getJSON(url, function (result) {
                if (result.length) {
                    updateDivisionDropdowns(true);

                } else {
                    updateDivisionDropdowns(false);

                    setAutocomplete();

                    $beachNameContainer.show();
                }

                gMapManager.setMapByAddress(currentAddress);
                
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo($nextDivision);
                });
            });

            function updateDivisionDropdowns(showingClosest) {
                $nextDivision.closest(divisionHolder).show();

                for (var i = nextDivision; i < $ddlDivisions.length; i++) {
                    $($ddlDivisions[i]).empty();
                    createInitialOption($($ddlDivisions[i]), '-- Choose an area --');

                    if (!showingClosest) {
                        $($ddlDivisions[i]).closest(divisionHolder).hide();
                    }
                }

                for (var i = nextDivision + 1; i < $ddlDivisions.length; i++) {
                    $($ddlDivisions[i]).closest(divisionHolder).hide();
                }
            }

            function setAutocomplete() {
                var secondSlashIndex = url.indexOf('/', 1);
                var controller = url.substr(0, secondSlashIndex);

                $textBoxName.autocomplete({
                    source: function (request, response) {
                        $.get(controller + '/BeachNames/', {
                            id: $this.val(),
                            term: request.term
                        }, function (data) {
                            response(data);
                        });
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $('<li class="ui-state-disabled">' + item.label + '</li>').appendTo(ul);
                }
            }

            function createInitialOption(jQueryAppendee, text) {
                $(document.createElement('option'))
                            .attr('value', "")
                            .text(text)
                            .appendTo(jQueryAppendee);
            }

            function getCurrentAddress() {
                var currentAddress = '';

                for (var i = 0; i < $ddlDivisions.length; i++) {
                    var $division = $($ddlDivisions[i])
                    var selector = '#' + $($ddlDivisions[i]).attr('id') + ' option:selected';
                    var text = $(selector).text();
                    var validText = (text && text.length > 0 && text.indexOf('-- Choose') === -1 && text.indexOf('undefined') === -1);

                    if (validText) {
                        currentAddress += text + ' ';
                    }
                }

                return currentAddress;
            }
        });

        $('#map').on('click', function () {
            var coordObj = gMapManager.getCoordinates();
            var coordinates = (coordObj.lat() + ',' + coordObj.lng());

            $textBoxCoordinates.val(coordinates);
        });

        $textBoxCoordinates.on('change', function () {
            if (!coordinatesValid()) {
                return;
            }

            var coordinates = $(this).val();

            gMapManager.setMapByCoord(coordinates);
        });        
    }

    function setEditEvents() {
        if (adding) {
            return;
        }

        //setImages();

        function setImages() {
            var $hdnImagePaths = $('[data-hdn-image-paths]');
            var imagePaths = $hdnImagePaths.data('hdn-image-paths').split(',');

            $hdnImagePaths.val('');

            for (var i = 0; i < imagePaths.length; i++) {
                var div = $('<div class="img-container">');
                var checkBox = $('<input />', { type: 'checkbox', value: imagePaths[i].substr(imagePaths[i].length - 25) });
                var a = $('<a>');
                var img = $('<img>');

                a.attr('href', imagePaths[i]);
                a.attr('data-lightbox', 'uploaded-images');
                img.attr('src', imagePaths[i]);

                checkBox.appendTo(div);
                img.appendTo(a);
                a.appendTo(div);
                div.appendTo('.uploaded-images-container');

                attachClickEventToCheckBox(checkBox);
            }

            function attachClickEventToCheckBox(checkBox) {
                checkBox.on('click', function () {
                    var $this = $(this);
                    var currentVal = $hdnImagePaths.val();

                    if ($this.is(':checked')) {
                        $hdnImagePaths.val(currentVal + ',' + $(this).val());

                        return;
                    }

                    $hdnImagePaths.val(currentVal.replace($this.val(), ''));
                })
            }
        }        
    }
})(jQuery);