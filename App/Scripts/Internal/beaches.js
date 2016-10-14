var GoogleMapManager = function () {
    var map;
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

        jQuery('[data-hidden-coordinates]').val(marker.position);

        getGeocoderData();
    }

    function removeMarkers() {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
    }

    function getGeocoderData() {
        var geocoder = new google.maps.Geocoder();
        var position = getMarkerCoordinates();
        var latLngObj = { lat: position.lat(), lng: position.lng() };
        geocoder.geocode({ 'location': latLngObj }, function (results, status) {
            console.log(results);
        });
    }
        
    return {
        initMap: initMap,
        getBeachCoordinates: getMarkerCoordinates
    }
}

var gMapManager = new GoogleMapManager();