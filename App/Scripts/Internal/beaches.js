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
    var $textBoxLocation = $('[data-textbox-location-name]');
    var $ddlWaterBody = $('[data-ddl-water-body]');
    var $textBoxCoordinates = $('[data-textbox-coordinates]');
    var $validationSpan = $('[data-generic-validation-alert]');
    var waterBodyValid = false;
    var waterBodyName;
    var waterBodyId;
    var locations = [];
    var waterBodies = [];
    var googleFailCounter = 0;

    setBindings();
    setAutocomplete();
    setEvents();
    setFileUpload();

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
            source: function (request, response) {
                $.get('Locations', {
                    term: request.term
                }, function (data) {
                    response(data);

                    locations = data;
                });
            },
            minLength: 2,
            autoFocus: true
        });

        $ddlWaterBody.autocomplete({
            source: function (request, response) {
                $.get('WaterBodies', {
                    term: request.term
                }, function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Name,
                            value: item.Id
                        };
                    }));

                    waterBodies = $.map(data, function (i) {
                        return i.Name;
                    });
                })
            },
            autoFocus: true,
            select: function (event, ui) {
                event.preventDefault();

                waterBodyId = ui.item.value;

                $ddlWaterBody.val(ui.item.label);
            }
        });
    }

    function setEvents() {
        $textBoxLocation.on('keydown', function (event) {
            if (event.which === 9) {
                if (locations.length) {
                    $textBoxLocation.val(locations[0]);
                }
            }
        });

        $ddlWaterBody.on('focusout', function () {
            var waterBodiesToLower = $.map(waterBodies, function (i) {
                return i.toLowerCase();
            });

            if (waterBodyName) {
                waterBodyValid = ($.inArray(waterBodyName.toLowerCase(), waterBodiesToLower) > -1);
            }

            $ddlWaterBody.val(waterBodies[0]); // Fill the body of water field with the first avaialble value in case the user changes focus
            waterBodyName = $ddlWaterBody.val();

            if (waterBodies.length && !waterBodyValid) {
                waterBodyValid = true; // We always have a valid value if waterBodies isn't empty
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

        $('[data-btn-create-beach]').on('click', function (event) {
            event.preventDefault();

            $validationSpan.text('');

            if (!validateForm($addBeachForm)) {
                return;
            }

            var locationData = gMapManager.getLocationData();
            var countryName;

            if (locationData) {
                var approximateAddressTokens = locationData.approximateAddress.split(',');
                countryName = approximateAddressTokens[approximateAddressTokens.length - 1].trim();
            } else {
                googleFailCounter++;

                if (googleFailCounter <= 1) {
                    $validationSpan.text("We coulnd't gather the geodata. Please try again.");
                } else {
                    $validationSpan.text("Something went wrong. Please select a country manually.");

                    $('[data-countries-form-block]').show();
                }

                return;
            }

            getUploadedFiles(makeServerRequest); // Make the AJAX request after we've read the uploaded files

            function makeServerRequest(uploadedFile) {
                var beachJsonData = {
                    name: $textBoxName.val(),
                    locationName: $textBoxLocation.val(),
                    description: $('[data-textbox-description]').val(),
                    waterBodyId: waterBodyId,
                    waterBodyName: waterBodyName,
                    countryName: (googleFailCounter <= 1) ? countryName : $('[data-ddl-country-name]').val(),
                    approximateAddress: locationData.approximateAddress,
                    coordinates: locationData.coordinates,
                    image: uploadedFile
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
                        if (result.redirectUrl) {
                            window.location.href = result.redirectUrl;
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        if (jqXHR.status === 412) {
                            $validationSpan.text(jqXHR.statusText);
                        } else {
                            $validationSpan.text('Something went wrong. We will look into it.');

                            // TO-DO CONTROLLER
                        }
                    }
                });
            }            
        });

        function validateForm() {
            $.validator.unobtrusive.parse($addBeachForm);

            var validator = $addBeachForm.validate();

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

        function getUploadedFiles(callback) {            
            var fileReader = new FileReader();

            fileReader.onloadend = function (e) {
                callback(e.target.result);
            };

            fileReader.readAsDataURL($('#uploader')[0].files[0]);
        }
    }

    function setFileUpload() {
        if (window.FileReader != null) {
            $('#file-reader-not-supported-box').hide();
        } else {
            $('#uploader-box').hide();
            $('.upload').prop('disabled', true);
        }
    }
})(jQuery);