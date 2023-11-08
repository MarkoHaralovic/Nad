let rezZanr = 0;
let rezTop = 0;
let logIn = JSON.parse(localStorage.getItem("logIn"));
let uredKn = JSON.parse(localStorage.getItem("urediKnjigu"));
var kos = JSON.parse(localStorage.getItem("obrisani"));

let btnZarn = document.getElementById('zarn');
let mapa = document.getElementById('mapa');
let logBtn = document.getElementById('logInBtn');

let listaZ = document.getElementById('listaZanr');
let listaT = document.getElementById('listaTop');

let plusLog = document.getElementById("plus");
let logoLog = document.getElementById("logIn");

let log = function() {

    if (logIn == undefined) {
    
        logIn = 0;
    }

    if (logIn == 0) {
        
        plusLog.style.display = "none";
        logoLog.style.display = "flex";

    } else if (logIn == 1) {
    
        plusLog.style.display = "flex";
        logoLog.style.display = "none";
    }

    console.log(logIn + " logIn");
}

let login = function() {

    logIn = 1;
    localStorage.setItem("logIn", JSON.stringify(logIn));
    log();
    window.location.href = "/Nad/Nad/izvorniKod/views/index.html";
}

let logout = function() {

    logIn = 0;
    localStorage.setItem("logIn", JSON.stringify(logIn));
    log();
    window.location.href = "/Nad/Nad/izvorniKod/views/index.html";
}
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

let brisiKnjigu = function() {

    uredKn = 1;
    localStorage.setItem("urediKnjigu", JSON.stringify(uredKn));

    window.location.href = "/Nad/Nad/izvorniKod/views/urediKnjigu.html";
}

let urediKnjigu = function() {

    uredKn = 2;
    localStorage.setItem("urediKnjigu", JSON.stringify(uredKn));
    window.location.href = "/Nad/Nad/izvorniKod/views/urediKnjigu.html";
}

let dodajKnjigu = function() {

    uredKn = 3;
    localStorage.setItem("urediKnjigu", JSON.stringify(uredKn));
    window.location.href = "/Nad/Nad/izvorniKod/views/promjenaKnjige.html";
}

log();