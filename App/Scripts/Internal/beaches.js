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
}

var gMapManager = new GoogleMapManager();

(function ($) {
    var $addBeachForm = $('#addBeachForm');
    var $ddlWaterBody = $('[data-ddl-water-body]');
    var $textBoxName = $('[data-textbox-name]');
    var $textBoxCoordinates = $('[data-textbox-coordinates]');
    var $validationSpan = $('[data-generic-validation-alert]');
    var waterBodyValid = false;
    var waterBodies = [];
    var previousNameValue;

    setBindings();
    setAutocomplete();
    setEvents();

    function setBindings() {
        $textBoxName.bind('paste', function (event) {
            event.preventDefault();
        });

        $ddlWaterBody.bind('paste', function (event) {
            event.preventDefault();
        });
    }

    function setAutocomplete() {
        $textBoxName.autocomplete({
            source: 'Names',
            minLength: 2
        }).data("ui-autocomplete")._renderItem = function (ul, item) {
            return $('<li class="ui-state-disabled">' + item.label + '</li>').appendTo(ul);
        };

        $('[data-textbox-location-name]').autocomplete({
            source: 'Locations',
            minLength: 2
        });

        $ddlWaterBody.autocomplete({
            source: function (request, response) {
                $.get('WaterBodies', {
                    term: request.term
                }, function (data) {
                    response(data);

                    waterBodies = data;
                });
            }
        });
    }

    function setEvents() {
        $ddlWaterBody.on('focusout', function () {
            var waterBodiesToLower = $.map(waterBodies, function (i) {
                return i.toLowerCase();
            });

            waterBodyValid = ($.inArray($ddlWaterBody.val().toLowerCase(), waterBodiesToLower) > -1);

            if (waterBodies.length && !waterBodyValid) {
                $ddlWaterBody.val(waterBodies[0]); // Fill the body of water field with the first avaialble value in case the user changes focus

                waterBodyValid = true;
            }
        });

        $('#map').on('click', function () {
            var coordObj = gMapManager.getCoordinates();
            var coordinates = (coordObj.lat() + ',' + coordObj.lng());

            $('[data-textbox-coordinates').val(coordinates);
        });

        $textBoxCoordinates.on('change', function () {
            if (!coordinatesValid()) {
                return;
            }

            var coordinates = $(this).val();

            gMapManager.setMap(coordinates);
        });

        $('[data-btn-create-beach]').on('click', function (event) {
            event.preventDefault();

            $validationSpan.text('');

            if (!validateForm($addBeachForm)) {
                return;
            }

            var locationData = gMapManager.getLocationData();

            if (!locationData) {
                $validationSpan.text("We coulnd't gather all the data. Please try again.");

                return;
            }

            var beachJsonData = {
                name: $textBoxName.val(),
                locationName: $('[data-textbox-location-name]').val(),
                description: $('[data-textbox-description]').val(),
                waterBody: $ddlWaterBody.val(),
                approximateAddress: locationData.approximateAddress,
                coordinates: locationData.coordinates
            };
            var csrfToken = $('input[name="__RequestVerificationToken"]', $addBeachForm).val();

            $.ajax({
                url: '/Beaches/Add/',
                type: 'POST',
                data: {
                    __RequestVerificationToken: csrfToken,
                    bindingModel: beachJsonData
                },
                success: function (result) {
                    if (result.data.indexOf('already exists') > -1) {
                        $validationSpan.text(result.data);
                    } else if (result.data.indexOf('Reviews/Rate' > -1)) {
                        window.location.href = result.data;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status === 412) {
                        $validationSpan.text('A beach with this name already exiss.');
                    } else {
                        $validationSpan.text('Something went wrong. We will look into it.');

                        // TO-DO CONTROLLER
                    }
                }
            });
        });
    }

    function validateForm() {
        $.validator.unobtrusive.parse($addBeachForm);

        $addBeachForm.validate();

        if (!$addBeachForm.valid()) {
            return false;
        }

        if (!gMapManager.getCoordinates()) {
            $validationSpan.text('Please select a beach on the map or input the coordinates manually.')

            return false;
        }

        if (!coordinatesValid()) {
            $validationSpan.text('Invalid coordinates.')

            return false;
        }

        if (!waterBodyValid) {
            $validationSpan.text('The body of water appears to be spelled incorrectly.')

            return false;
        }

        return true;
    }

    function coordinatesValid() {
        var pattern = /^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$/;

        if (!pattern.test($textBoxCoordinates.val())) {
            return false;
        }

        return true;
    }
})(jQuery);