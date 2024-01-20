var map = L.map('map').setView([44.22, 18.3], 7);
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

fetch("/markers")
    .then(x => x.json())
    .then(markers => {
        for (let marker of markers) {
            let leafletMarker = L.marker([marker.latitude, marker.longitude]).addTo(map);
            leafletMarker.bindPopup(`<a href='/account/profile/${marker.userId}'>${marker.displayName}</a>`)
        }        
    });