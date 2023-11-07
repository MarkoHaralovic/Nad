let rezZanr = 0;
let rezTop = 0;

let btnZarn = document.getElementById('zarn');
let mapa = document.getElementById('mapa');

let listaZ = document.getElementById('listaZanr');
let listaT = document.getElementById('listaTop');


let showZanr = function() {

    if (rezZanr == 0) {
    
        mapa.style.display = "none";
        listaZ.style.display = "flex"
        listaT.style.display = "none"
        rezZanr = 1;
        rezTop = 0;

    } else if (rezZanr == 1) {
    
        mapa.style.display = "flex";
        listaZ.style.display = "none";
        listaT.style.display = "none"
        rezZanr = 0;
        rezTop = 0;
    }
};
let showTop = function() {

    if (rezTop == 0) {
    
        mapa.style.display = "none";
        listaZ.style.display = "none"
        listaT.style.display = "flex"
        rezZanr = 0;
        rezTop = 1;

    } else if (rezTop == 1) {
    
        mapa.style.display = "flex";
        listaZ.style.display = "none";
        listaT.style.display = "none"
        rezZanr = 0;
        rezTop = 0;
    }
};