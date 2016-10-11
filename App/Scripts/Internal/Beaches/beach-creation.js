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
    }

    function removeMarkers() {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
    }

    function getMarkerCoordinates() {
        return (markers.length ? markers[0].position : "Nothing selected.");
    }
        
    return {
        initMap: initMap,
        getBeachCoordinates: getMarkerCoordinates
    }
}

var gMapManager = new GoogleMapManager();