var GoogleMapManager = function () {
    var map,
        geocoderFilteredResults;
    var markers = [];

    function initMap() {
        var initialPosition = new google.maps.LatLng(-34.397, 150.644);
        var mapOptions = {
            zoom: 8,
            center: initialPosition,
            mapTypeId: 'satellite'
        };
        map = new google.maps.Map(document.getElementById('map'), mapOptions);

        attachEventListeners();
    }

    function getGeocoderData() {
        var geocoder = new google.maps.Geocoder();
        var position = getMarkerCoordinates();

        if (!position) {
            return false;
        }

        var latLngObj = { lat: parseFloat(position.lat()), lng: parseFloat(position.lng()) };

        geocoder.geocode({ 'location': latLngObj }, function (results, status) {
            switch (status) {
                case 'OK':
                    geocoderFilteredResults = {
                        approximateAddress: results[0].formatted_address,
                        coordinates: (position.lat() + ',' + position.lng())
                    };
                default:
                    return false;
            }
        });

        return geocoderFilteredResults;
    }

    function shiftMap(coordinates) {
        var latLng = coordinates.split(',', 2);

        removeMarkers();

        map.setCenter({ lat: parseFloat(latLng[0]), lng: parseFloat(latLng[1]) });
    }

    function getMarkerCoordinates() {
        if (!markers.length) {
            return false;
        }

        return markers[0].position;
    }

    function attachEventListeners() {
        google.maps.event.addListener(map, "click", function (event) {
            removeMarkers();

            addMarker(map, event.latLng);
        });
    }

    function addMarker(map, position) {
        var marker = new google.maps.Marker({
            position: position,
            map: map
        });

        markers.push(marker);
    }

    function removeMarkers() {
        markers.forEach(function (marker) {
            marker.setMap(null);
        });

        markers = [];
    }

    return {
        initMap: initMap,
        getLocationData: getGeocoderData,
        getCoordinates: getMarkerCoordinates,
        setMap: shiftMap
    }
};

var gMapManager = new GoogleMapManager();

(function ($) {
    var $ddlDivisions = $('[data-ddl-division]');
    var $textBoxName = $('[data-textbox-name]');
    var $beachNameContainer = $('[data-beach-name]');
    var $textBoxCoordinates = $('[data-textbox-coordinates]');
    var $validationSpan = $('[data-generic-validation-alert]');
    
    resetCountriesDropdown();
    setBindings();
    setEvents();

    function resetCountriesDropdown() {
        if ($($ddlDivisions[0]).val()) {
            $($ddlDivisions[0]).val('');
        }
    }

    function setBindings() {
        $textBoxName.bind('paste', function (event) {
            event.preventDefault();
        });
    }    

    function setEvents() {
        $ddlDivisions.on('change', function () {
            var $this = $(this);            
            var divisionHolder = '[data-division-holder]';
            var nextDivision = $this.data('next-division');
            var url = $this.data('ddl-division') + $this.val();

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

                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo($nextDivision);
                });
            });

            function updateDivisionDropdowns(showing) {
                $nextDivision.closest(divisionHolder).show();

                for (var i = nextDivision; i < $ddlDivisions.length; i++) {
                    $($ddlDivisions[i]).empty();
                    createInitialOption($($ddlDivisions[i]), '-- Choose an area --');

                    if (!showing) {
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

            gMapManager.setMap(coordinates);
        });        
    }
})(jQuery);