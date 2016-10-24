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
    var $ddlRegions = $('[data-ddl-region-name]');
    var $ddlAreas = $('[data-ddl-area-name]');
    var $areasContainer = $('[data-areas]');
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
                $.get('/Areas/BeachNames/', {
                    id: $ddlAreas.val(),
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
            var url = '/Countries/Regions/' + $ddlCountries.val();

            $ddlRegions.empty();
            $ddlAreas.empty();

            createInitialOption($ddlRegions, '-- Choose a region/state --');

            $.getJSON(url, function (result) {
                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo($ddlRegions);
                });
            });

            $('[data-regions]').show();
            $areasContainer.hide();
        });

        $ddlRegions.on('change', function () {
            var regionId = $ddlRegions.val();
            var areasUrl = '/Regions/Areas/' + regionId;
            var waterBodyUrl = '/Regions/WaterBody/' + regionId;

            $.getJSON(waterBodyUrl, function (result) {
                $('[data-hdn-water-body-id]').val(result);
            });

            $.getJSON(areasUrl, function (result) {
                $ddlAreas.empty();

                createInitialOption($ddlAreas, '-- Choose an area --');

                $(result).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Value)
                        .text(this.Text)
                        .appendTo($ddlAreas);
                });
            });

            $areasContainer.show();
            $beachNameContainer.hide();
        });

        $ddlAreas.on('change', function () {
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