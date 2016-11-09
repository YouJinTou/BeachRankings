var GoogleMapManager = function () {
    var map,
        geocoder,
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
        geocoder = new google.maps.Geocoder();

        attachEventListeners();
    }

    function getGeocoderData() {
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

    function shiftMapByCoord(coordinates) {
        var latLng = coordinates.split(',', 2);

        removeMarkers();

        map.setCenter({ lat: parseFloat(latLng[0]), lng: parseFloat(latLng[1]) });
    }

    function shiftMapByAddress(address) {
        geocoder.geocode({ address: address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);//center the map over the result                
            }
        });
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
        setMapByCoord: shiftMapByCoord,
        setMapByAddress: shiftMapByAddress
    }
};

var gMapManager = new GoogleMapManager();