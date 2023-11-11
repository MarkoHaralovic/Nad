var map = L.map('reg-map').setView([44.22, 18.3], 7); 
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

var marker;

function onMapClick(e) {
    if(marker) {
        map.removeLayer(marker);
    }
    marker = L.marker(e.latlng).addTo(map);
    
}

map.on('click',onMapClick);

let sendButton = document.getElementById('send-location');
document.getElementById('send-location').addEventListener('click', function () {
    if (marker) {
        var latlng = marker.getLatLng();
        alert('Latitude: ' + latlng.lat + ', Longitude: ' + latlng.lng);
        // You can send the location data to your server or handle it as needed.
        window.marker = marker;
    } else {
        alert('Please mark a location on the map first.');
    }
});