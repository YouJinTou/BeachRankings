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
        var pattern = /^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$/;
        
        if (!pattern.test(coordinates)) {
            return;
        }

        var latLng = coordinates.split(',', 2);

        removeMarkers();

        map.setCenter({ lat: parseFloat(latLng[0]), lng: parseFloat(latLng[1]) });
    }

    function getMarkerCoordinates() {
        if (!markers.length) {
            return "Nothing selected.";
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
}

var gMapManager = new GoogleMapManager();

(function ($) {
    $('#map').on('click', function () {
        var coordObj = gMapManager.getCoordinates();
        var coordinates = (coordObj.lat() + ',' + coordObj.lng());

        $('[data-textbox-coordinates').val(coordinates);
    });

    $('[data-textbox-coordinates]').on('change', function () {
        var coordinates = $(this).val();

        gMapManager.setMap(coordinates);
    })

    $('[data-btn-create-beach]').on('click', function (event) {
        event.preventDefault();

        var locationData = gMapManager.getLocationData();

        if (!locationData) {
            return false;
        }

        var beachJsonData = {
            name: $('[data-textbox-name]').val(),
            locationName: $('[data-textbox-location-name]').val(),
            description: $('[data-textbox-description]').val(),
            waterBody: $('[data-textbox-water-body]').val(),
            approximateAddress: locationData.approximateAddress,
            coordinates: locationData.coordinates
        };
        var addBeachForm = $('#addBeachForm');
        var csrfToken = $('input[name="__RequestVerificationToken"]', addBeachForm).val();

        if (addBeachForm.valid()) {
            $.ajax({
                url: '/Beaches/Add/',
                type: 'POST',
                data: {
                    __RequestVerificationToken: csrfToken,
                    bindingModel: beachJsonData
                },
                success: function (result) {
                    if (result.redirectUrl) {
                        window.location.href = result.redirectUrl;
                    }
                }
            });
        }
    });
})(jQuery);