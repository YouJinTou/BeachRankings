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
    var $addBeachForm = $('#addBeachForm');
    var $textBoxName = $('[data-textbox-name]');
    var $ddlCountries = $('[data-ddl-country-name]');
    var $ddlPrimaryDivisions = $('[data-ddl-primary-division-name]');
    var $ddlSecondaryDivisions = $('[data-ddl-secondary-division-name]');
    var $secondaryDivisionsContainer = $('[data-secondary-divisions]');
    var $beachNameContainer = $('[data-beach-name]');
    var $textBoxCoordinates = $('[data-textbox-coordinates]');
    var $validationSpan = $('[data-generic-validation-alert]');
    
    resetCountriesDropdown();
    setBindings();
    setAutocomplete();
    setEvents();

    function resetCountriesDropdown() {
        if ($ddlCountries.val()) {
            $ddlCountries.val('');
        }
    }

    function setBindings() {
        $textBoxName.bind('paste', function (event) {
            event.preventDefault();
        });
    }

    function setAutocomplete() {
        $textBoxName.autocomplete({
            source: function (request, response) {
                $.get('/SecondaryDivisions/BeachNames/', {
                    id: $ddlSecondaryDivisions.val(),
                    term: request.term
                }, function (data) {
                    response(data);
                });
            }
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            return $('<li class="ui-state-disabled">' + item.label + '</li>').appendTo(ul);
        }
    }

    function setEvents() {
        $ddlCountries.on('change', function () {
            var url = '/Countries/PrimaryDivisions/' + $ddlCountries.val();

            $ddlPrimaryDivisions.empty();
            $ddlSecondaryDivisions.empty();

            createInitialOption($ddlPrimaryDivisions, '-- Choose a region/state --');

            $.getJSON(url, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo($ddlPrimaryDivisions);
                });
            });

            $('[data-primary-divisions]').show();
            $secondaryDivisionsContainer.hide();
        });

        $ddlPrimaryDivisions.on('change', function () {
            var primaryDivisionId = $ddlPrimaryDivisions.val();
            var areasUrl = '/PrimaryDivisions/SecondaryDivisions/' + primaryDivisionId;
            var waterBodyUrl = '/PrimaryDivisions/WaterBody/' + primaryDivisionId;

            $.getJSON(waterBodyUrl, function (result) {
                $('[data-hdn-water-body-id]').val(result);
            });

            $.getJSON(areasUrl, function (result) {
                $ddlSecondaryDivisions.empty();

                createInitialOption($ddlSecondaryDivisions, '-- Choose an option --');

                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo($ddlSecondaryDivisions);
                });
            });

            $secondaryDivisionsContainer.show();
            $beachNameContainer.hide();
        });

        $ddlSecondaryDivisions.on('change', function () {
            $textBoxName.val('');
            $beachNameContainer.show();
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

        function createInitialOption(jQueryAppendee, text) {
            $(document.createElement('option'))
                        .attr('value', "")
                        .text(text)
                        .appendTo(jQueryAppendee);
        }
    }
})(jQuery);